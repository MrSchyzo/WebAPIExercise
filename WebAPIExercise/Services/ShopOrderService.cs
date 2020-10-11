using WebAPIExercise.Data;
using AutoMapper;
using System;
using System.Linq;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InOrder = WebAPIExercise.Input.Order;
using DbOrder = WebAPIExercise.Data.Models.Order;
using DbOrderItem = WebAPIExercise.Data.Models.OrderItem;
using DbProduct = WebAPIExercise.Data.Models.Product;

namespace WebAPIExercise.Services
{
    public class ShopOrderService : IOrderService
    {
        private readonly ShopContext shop;
        private readonly IMapper mapper;

        public ShopOrderService(ShopContext shop, IMapper mapper)
        {
            this.shop = shop;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbOrder> products = await shop.Orders.Skip(pageStart * pageSize).Take(pageSize).ToListAsync();

            return products.Select(mapper.Map<Order>);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            DbOrder found = await shop.Orders.Where(order => order.Id == id).FirstAsync();

            return mapper.Map<Order>(found);
        }

        public async Task<Order> NewAsync(InOrder order)
        {
            ImmutableHashSet<int> productIds = order.Items.Select(item => item.ProductId).ToImmutableHashSet();

            using (var transaction = await shop.Database.BeginTransactionAsync())
            {
                IDictionary<int, DbProduct> products = await shop.Products.Where(prod => productIds.Contains(prod.Id)).ToDictionaryAsync(prod => prod.Id);
                DbOrder newOrder = (await shop.Orders.AddAsync(OrderFrom(order, products))).Entity;

                await foreach (var o in shop.Orders.Where(o => o.CompanyCode == order.CompanyCode).AsAsyncEnumerable())
                {
                    if ((DateTime.Now - o.Date).Days != 0) continue;
                    await transaction.RollbackAsync();
                    throw new Exception("Cannot accept another order from the same company on the same day");
                }

                if (order.Items.Sum(item => products[item.ProductId].UnitPrice * item.OrderedQuantity) < 100.0)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Cannot accept orders for less than 100.0");
                }

                if (order.Items.Any(item => products[item.ProductId].StockQuantity < item.OrderedQuantity))
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Cannot accept order as there is some shortage in the ordered products");
                }

                foreach (var item in order.Items)
                {
                    products[item.ProductId].StockQuantity -= item.OrderedQuantity;
                }

                await shop.SaveChangesAsync();
                await transaction.CommitAsync();

                return mapper.Map<Order>(newOrder);
            }
        }

        private static DbOrder OrderFrom(InOrder order, IDictionary<int, DbProduct> products)
        {
            return new DbOrder
            {
                CompanyCode = order.CompanyCode,
                Date = DateTime.Now,
                OrderItems = order.Items.Select(item =>
                    new DbOrderItem
                    {
                        OrderedQuantity = item.OrderedQuantity,
                        Product = products[item.ProductId]
                    }
                ).ToList()
            };
        }
    }
}
