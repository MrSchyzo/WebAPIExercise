using Functional.Maybe;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetPage(int start, int size);
        public Task<Maybe<Order>> GetById(int id);
        public Task<Order> NewOrder(Order order);
        public Task<bool> HasCompanyOrdersForToday(Order order);
    }
}