using Functional.Maybe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    /// <summary>
    /// <inheritdoc cref="IOrderRepository"/>
    /// </summary>
    public class ShopOrderRepository : IOrderRepository
    {
        private readonly ShopContext context;

        public ShopOrderRepository(ShopContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <inheritdoc cref="IOrderRepository.GetById(int)"/>
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>A Maybe monad wrapping the value; the monad is empty if the record is not found</returns>
        public async Task<Maybe<Order>> GetById(int id)
        {
            return (
                await context.Orders
                        .Include(order => order.OrderItems)
                        .ThenInclude(item => item.Product)
                        .Where(order => order.Id == id)
                        .FirstOrDefaultAsync()
            ).ToMaybe();
        }

        /// <summary>
        /// <inheritdoc cref="IOrderRepository.GetPage(int, int)"/>
        /// </summary>
        /// <param name="start">0-based index of the page</param>
        /// <param name="size">size of the page</param>
        /// <returns>A paged chunk of Orders</returns>
        public async Task<IEnumerable<Order>> GetPage(int start, int size)
        {
            return 
                await context.Orders
                        .Include(order => order.OrderItems)
                        .ThenInclude(item => item.Product)
                        .Skip(start * size)
                        .Take(size)
                        .ToListAsync();
        }

        /// <summary>
        /// <inheritdoc cref="IOrderRepository.HasCompanyOrdersForToday(Order)"/>
        /// </summary>
        /// <param name="order">Order to check condition on</param>
        /// <returns>True if an Order already exists; false otherwise</returns>
        public async Task<bool> HasCompanyOrdersForToday(Order order)
        {
            DateTime now = DateTime.Now;
            return await context.Orders.Where(o => o.CompanyCode == order.CompanyCode && o.Date.Date == now.Date).AnyAsync();
        }

        /// <summary>
        /// <inheritdoc cref="IOrderRepository.NewOrder(Order)"/>
        /// </summary>
        /// <param name="order">Product Entity to save</param>
        /// <returns>The newly created entity with the new ID</returns>
        public async Task<Order> NewOrder(Order order)
        {
            Order newOrder = (await context.Orders.AddAsync(order)).Entity;
            await context.OrderItems.AddRangeAsync(newOrder.OrderItems);
            await context.SaveChangesAsync();
            return newOrder;
        }
    }
}