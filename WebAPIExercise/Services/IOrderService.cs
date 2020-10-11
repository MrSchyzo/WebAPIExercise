using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InOrder = WebAPIExercise.Input.Order;

namespace WebAPIExercise.Services
{
    public interface IOrderService
    {
        public Task<Order> GetByIdAsync(int id);
        public Task<IEnumerable<Order>> GetAllPagedAsync(int pageStart, int pageSize);
        public Task<Order> NewAsync(InOrder order);
    }
}
