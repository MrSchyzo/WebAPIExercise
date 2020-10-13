using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InProduct = WebAPIExercise.Input.Product;

namespace WebAPIExercise.Services
{
    /// <summary>
    /// Handles the business logic for everything concerning Products
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Asynchronously gets all Products in a chosen page.
        /// </summary>
        /// <param name="pageStart">0-based index of the page</param>
        /// <param name="pageSize">Size of the page</param>
        /// <returns>All Products in a chosen page</returns>
        public Task<Product> GetByIdAsync(int id);
        /// <summary>
        /// Asynchronously gets a Product by id
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>A Product by id</returns>
        public Task<IEnumerable<Product>> GetAllPagedAsync(int pageStart, int pageSize);
        /// <summary>
        /// Asynchronously creates and returns a new Product from a given POCO.
        /// </summary>
        /// <param name="product">POCO representing the Product to save</param>
        /// <returns>POCO representing the output Product</returns>
        public Task<Product> NewAsync(InProduct product);
    }
}
