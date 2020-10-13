using Functional.Maybe;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    /// <summary>
    /// A repository regarding Products for trivial operations in the dataset
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Returns a paged chunk of Products
        /// </summary>
        /// <param name="start">0-based index of the page</param>
        /// <param name="size">size of the page</param>
        /// <returns>A paged chunk of Products</returns>
        public Task<IEnumerable<Product>> GetPage(int start, int size);
        /// <summary>
        /// Returns a Product by id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>A Maybe monad wrapping the value; the monad is empty if the record is not found</returns>
        public Task<Maybe<Product>> GetById(int id);
        /// <summary>
        /// Adds a Product into the dataset
        /// </summary>
        /// <param name="product">Product Entity to save</param>
        /// <returns>The newly created entity with the new ID</returns>
        public Task<Product> NewProduct(Product product);
        /// <summary>
        /// Checks whether this Product has colliding name and description with something already into the dataset.
        /// </summary>
        /// <param name="product">Product to check collisions on</param>
        /// <returns>True if there is already a Product with those name and description</returns>
        public Task<bool> IsThereAnyCollisionWith(Product product);
        /// <summary>
        /// Returns a directly accessible collection of Products found in a provided collection of IDs
        /// </summary>
        /// <param name="ids">Product IDs to search for</param>
        /// <returns>A Dictionary [productId => productEntity] such that productId is contained in the provided collection</returns>
        public Task<IDictionary<int, Product>> GetByIdIn(ICollection<int> ids);
        /// <summary>
        /// Modifies a Product stock by a given amount
        /// </summary>
        /// <param name="product">Entity to modify</param>
        /// <param name="amount">Amount to give</param>
        public Task DecrementStockBy(Product product, int amount);
    }
}