using Functional.Maybe;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    public class ShopProductRepository : IProductRepository
    {
        private readonly ShopContext context;
        
        public ShopProductRepository(ShopContext context)
        {
            this.context = context;
        }

        public async Task<Maybe<Product>> GetById(int id)
        {
            return (await context.Products.Where(product => product.Id == id).FirstOrDefaultAsync()).ToMaybe();
        }

        public async Task<bool> IsThereAnyCollisionsWith(Product product)
        {
            return await context.Products.Where(dbProd => product.Description == dbProd.Description && product.Name == dbProd.Name).AnyAsync();
        }

        public async Task<IEnumerable<Product>> GetPage(int start, int size)
        {
            return await context.Products.Skip(start * size).Take(size).ToListAsync();
        }

        public async Task<Product> NewProduct(Product product)
        {
            Product saved = (await context.Products.AddAsync(product)).Entity;
            await context.SaveChangesAsync();
            return saved;
        }

        public async Task<IDictionary<int, Product>> GetByIdIn(ICollection<int> ids)
        {
            return await context.Products.Where(prod => ids.Contains(prod.Id)).ToDictionaryAsync(prod => prod.Id);
        }

        public async Task DecrementStockBy(Product product, int amount)
        {
            product.StockQuantity -= amount;
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }
    }
}