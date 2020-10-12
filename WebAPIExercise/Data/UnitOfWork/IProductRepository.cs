using Functional.Maybe;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data.UnitOfWork
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetPage(int start, int size);
        public Task<Maybe<Product>> GetById(int id);
        public Task<Product> NewProduct(Product product);
        public Task<bool> IsThereAnyCollisionsWith(Product product);
        public Task<IDictionary<int, Product>> GetByIdIn(ICollection<int> ids);
        public Task DecrementStockBy(Product product, int amount);
    }
}