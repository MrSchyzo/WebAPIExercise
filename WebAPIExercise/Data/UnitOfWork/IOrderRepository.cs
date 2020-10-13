using Functional.Maybe;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    /// <summary>
    /// A repository regarding Orders (and related OrderItems) for trivial operations in the dataset
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Returns a paged chunk of Orders
        /// </summary>
        /// <param name="start">0-based index of the page</param>
        /// <param name="size">size of the page</param>
        /// <returns>A paged chunk of Orders</returns>
        public Task<IEnumerable<Order>> GetPage(int start, int size);
        /// <summary>
        /// Returns a Order by id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>A Maybe monad wrapping the value; the monad is empty if the record is not found</returns>
        public Task<Maybe<Order>> GetById(int id);
        /// <summary>
        /// Adds a Order into the dataset
        /// </summary>
        /// <param name="order">Product Entity to save</param>
        /// <returns>The newly created entity with the new ID</returns>
        public Task<Order> NewOrder(Order order);
        /// <summary>
        /// Checks whether the company of the given Order already has one in this day
        /// </summary>
        /// <param name="order">Order to check condition on</param>
        /// <returns>True if an Order already exists; false otherwise</returns>
        public Task<bool> HasCompanyOrdersForToday(Order order);
    }
}