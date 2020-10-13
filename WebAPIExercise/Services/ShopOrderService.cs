using AutoMapper;
using System;
using System.Linq;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;
using WebAPIExercise.Data.UnitOfWork;
using Functional.Maybe;
using WebAPIExercise.Utils;

using InOrder = WebAPIExercise.Input.Order;
using DbOrder = WebAPIExercise.Data.Models.Order;
using DbOrderItem = WebAPIExercise.Data.Models.OrderItem;
using DbProduct = WebAPIExercise.Data.Models.Product;
using WebAPIExercise.Errors;

namespace WebAPIExercise.Services
{
    public class ShopOrderService : IOrderService
    {
        private readonly ShopUnitOfWork unit;
        private readonly IMapper mapper;

        public ShopOrderService(ShopUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbOrder> orders = await unit.ExecuteAsync(async (_, orders) => await orders.GetPage(pageStart, pageSize));

            return orders.Select(mapper.Map<Order>);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            Maybe<DbOrder> maybe = await unit.ExecuteAsync(async (_, order) => await order.GetById(id));

            return maybe
                    .Select(mapper.Map<Order>)
                    .OrElseThrow(() => new NotFoundException($"Order {id} not found"));
        }

        public async Task<Order> NewAsync(InOrder order)
        {
            ImmutableHashSet<int> productIds = order.Items.Select(item => item.ProductId).ToImmutableHashSet();

            if (productIds.Count != order.Items.Count())
            {
                throw new InvalidEntityException("There is some duplicated product entry in current order, please merge for unique productId entries");
            }

            DbOrder newOrder = await unit.ExecuteAsync(async (prodRepo, orderRepo) =>
            {
                IDictionary<int, DbProduct> products = await prodRepo.GetByIdIn(productIds);

                DbOrder toInsert = OrderFrom(order, products);

                if (!productIds.All(products.ContainsKey))
                {
                    throw new InvalidEntityException("Cannot accept an order with invalid product ids");
                }
                if (await orderRepo.HasCompanyOrdersForToday(toInsert))
                {
                    throw new InvalidEntityException($"Today company {order.CompanyCode} has already ordered something");
                }
                if (order.Items.Sum(item => products[item.ProductId].UnitPrice * item.OrderedQuantity) < 100.0)
                {
                    throw new InvalidEntityException("Cannot accept orders for less than 100.0");
                }
                if (order.Items.Any(item => products[item.ProductId].StockQuantity < item.OrderedQuantity))
                {
                    throw new InvalidEntityException("Cannot accept order as there is some shortage in the ordered products");
                }

                DbOrder saved = await orderRepo.NewOrder(toInsert);

                await Task.WhenAll(order.Items.Select(item => prodRepo.DecrementStockBy(products[item.ProductId], item.OrderedQuantity)).ToArray());

                return saved;
            });

            return mapper.Map<Order>(newOrder);
        }

        private static DbOrder OrderFrom(InOrder order, IDictionary<int, DbProduct> products)
        {
            DbOrder newOrder = new DbOrder
            {
                CompanyCode = order.CompanyCode,
                Date = DateTime.Now
            };

            newOrder.OrderItems = order.Items.Select(item => new DbOrderItem{ OrderedQuantity = item.OrderedQuantity, Product = products[item.ProductId], Order = newOrder}).ToList();

            return newOrder;
        }
    }
}
