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
    /// <summary>
    /// <inheritdoc cref="IOrderService"/>
    /// <para>It uses a single DB UnitOfWork</para>
    /// </summary>
    public class ShopOrderService : IOrderService
    {
        private readonly ShopUnitOfWork unit;
        private readonly IMapper mapper;

        public ShopOrderService(ShopUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        /// <summary>
        /// <inheritdoc cref="IOrderService.GetAllPagedAsync(int, int)"/>
        /// <para>It does not check argument validity.</para>
        /// </summary>
        /// <param name="pageStart">0-based index of the page</param>
        /// <param name="pageSize">Size of the page</param>
        /// <returns>All Orders in a chosen page</returns>
        public async Task<IEnumerable<Order>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbOrder> orders = await unit.ExecuteAsync(async (_, orders) => await orders.GetPage(pageStart, pageSize));

            return orders.Select(mapper.Map<Order>);
        }

        /// <summary>
        /// <inheritdoc cref="IOrderService.GetByIdAsync(int)"/>
        /// <para>It throws NotFoundException if the Order is not found</para>
        /// </summary>
        /// <param name="id">Order identifier</param>
        /// <returns>An Order by id</returns>
        public async Task<Order> GetByIdAsync(int id)
        {
            Maybe<DbOrder> maybe = await unit.ExecuteAsync(async (_, order) => await order.GetById(id));

            return maybe
                    .Select(mapper.Map<Order>)
                    .OrElseThrow(() => new NotFoundException($"Order {id} not found"));
        }

        /// <summary>
        /// <inheritdoc cref="IOrderService.NewAsync(InOrder)"/>
        /// <para>InvalidEntityException is thrown if:</para>
        /// <list type="bullet">
        /// <item>Provided OrderItems are not unique by referenced ProductId</item>
        /// <item>Provided OrderItems have inexistent ProductIds</item>
        /// <item>There is an Order by the same company in the same day</item>
        /// <item>The Product total is under 100.0</item>
        /// <item>Some Product has a stock amount that is inferior to the ordered quantity</item>
        /// </list>
        /// </summary>
        /// <param name="product">POCO representing the Order to save</param>
        /// <returns>POCO representing the output Order</returns>
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
