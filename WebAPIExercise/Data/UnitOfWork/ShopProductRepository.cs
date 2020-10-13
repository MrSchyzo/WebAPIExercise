using Functional.Maybe;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    /// <summary>
    /// <inheritdoc cref="IProductRepository"/>
    /// </summary>
    public class ShopProductRepository : IProductRepository
    {
        private readonly ShopContext context;
        
        public ShopProductRepository(ShopContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.GetById(int)"/>
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>A Maybe monad wrapping the value; the monad is empty if the record is not found</returns>
        public async Task<Maybe<Product>> GetById(int id)
        {
            return (await context.Products.Where(product => product.Id == id).FirstOrDefaultAsync()).ToMaybe();
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.IsThereAnyCollisionWith(Product)"/>
        /// </summary>
        /// <param name="product">Product to check collisions on</param>
        /// <returns>True if there is already a Product with those name and description</returns>
        public async Task<bool> IsThereAnyCollisionWith(Product product)
        {
            return await context.Products.Where(dbProd => product.Description == dbProd.Description && product.Name == dbProd.Name).AnyAsync();
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.GetPage(int, int)"/>
        /// </summary>
        /// <param name="start">0-based index of the page</param>
        /// <param name="size">size of the page</param>
        /// <returns>A paged chunk of Products</returns>
        public async Task<IEnumerable<Product>> GetPage(int start, int size)
        {
            return await context.Products.Skip(start * size).Take(size).ToListAsync();
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.NewProduct(Product)"/>
        /// </summary>
        /// <param name="product">Product Entity to save</param>
        /// <returns>The newly created entity with the new ID</returns>
        public async Task<Product> NewProduct(Product product)
        {
            Product saved = (await context.Products.AddAsync(product)).Entity;
            await context.SaveChangesAsync();
            return saved;
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.GetByIdIn(ICollection{int})"/>
        /// </summary>
        /// <param name="ids">Product IDs to search for</param>
        /// <returns>A Dictionary [productId => productEntity] such that productId is contained in the provided collection</returns>
        public async Task<IDictionary<int, Product>> GetByIdIn(ICollection<int> ids)
        {
            return await context.Products.Where(prod => ids.Contains(prod.Id)).ToDictionaryAsync(prod => prod.Id);
        }

        /// <summary>
        /// <inheritdoc cref="IProductRepository.DecrementStockBy(Product, int)"/>
        /// </summary>
        /// <param name="product">Entity to modify</param>
        /// <param name="amount">Amount to give</param>
        public async Task DecrementStockBy(Product product, int amount)
        {
            product.StockQuantity -= amount;
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }
    }
}