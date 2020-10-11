using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIExercise.Output;

using InProduct = WebAPIExercise.Input.Product;

namespace WebAPIExercise.Services
{
    public interface IProductService
    {
        public Task<Product> GetByIdAsync(int id);
        public Task<IEnumerable<Product>> GetAllPagedAsync(int pageStart, int pageSize);
        public Task<Product> NewAsync(InProduct product);
    }
}
