using WebAPIExercise.Data;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InOrder = WebAPIExercise.Input.Order;
using DbOrder = WebAPIExercise.Data.Models.Order;

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
            throw new System.NotSupportedException();
        }
    }
}
