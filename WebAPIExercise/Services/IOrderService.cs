using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InOrder = WebAPIExercise.Input.Order;

namespace WebAPIExercise.Services
{
    /// <summary>
    /// Handles the business logic for everything concerning Orders and OrderItems.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Asynchronously gets an Order by id.
        /// </summary>
        /// <param name="id">Order identifier</param>
        /// <returns>An Order by id</returns>
        public Task<Order> GetByIdAsync(int id);
        /// <summary>
        /// Asynchronously gets all Orders in a chosen page.
        /// </summary>
        /// <param name="pageStart">0-based index of the page</param>
        /// <param name="pageSize">Size of the page</param>
        /// <returns>All Orders in a chosen page</returns>
        public Task<IEnumerable<Order>> GetAllPagedAsync(int pageStart, int pageSize);
        /// <summary>
        /// Asynchronously creates and returns a new Order from a given POCO.
        /// </summary>
        /// <param name="order">POCO representing the Order to save</param>
        /// <returns>Order representing the output Product</returns>
        public Task<Order> NewAsync(InOrder order);
    }
}
