using Functional.Maybe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    public class ShopOrderRepository : IOrderRepository
    {
        private readonly ShopContext context;

        public ShopOrderRepository(ShopContext context)
        {
            this.context = context;
        }

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

        public async Task<bool> HasCompanyOrdersForToday(Order order)
        {
            DateTime now = DateTime.Now;
            return await context.Orders.Where(o => o.CompanyCode == order.CompanyCode && o.Date.Date == now.Date).AnyAsync();
        }

        public async Task<Order> NewOrder(Order order)
        {
            Order newOrder = (await context.Orders.AddAsync(order)).Entity;
            await context.OrderItems.AddRangeAsync(newOrder.OrderItems);
            await context.SaveChangesAsync();
            return newOrder;
        }
    }
}