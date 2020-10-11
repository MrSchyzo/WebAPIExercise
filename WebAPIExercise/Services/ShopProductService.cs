using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Data;
using WebAPIExercise.Output;

using InProduct = WebAPIExercise.Input.Product;
using DbProduct = WebAPIExercise.Data.Models.Product;

namespace WebAPIExercise.Services
{
    public class ShopProductService : IProductService
    {
        private readonly ShopContext shop;
        private readonly IMapper mapper;

        public ShopProductService(ShopContext shop, IMapper mapper)
        {
            this.shop = shop;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbProduct> products = await shop.Products.Skip(pageStart * pageSize).Take(pageSize).ToListAsync();

            return products.Select(mapper.Map<Product>);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            DbProduct found = await shop.Products.Where(product => product.Id == id).FirstAsync();

            return mapper.Map<Product>(found);
        }

        public async Task<Product> NewAsync(InProduct product)
        {
            if (await shop.Products.Where(dbProd => product.Description == dbProd.Description && product.Name == dbProd.Name).AnyAsync())
            {
                throw new System.Exception("Product name and description already exist");
            }

            DbProduct newProduct = (await shop.Products.AddAsync(mapper.Map<DbProduct>(product))).Entity;

            await shop.SaveChangesAsync();

            return mapper.Map<Product>(newProduct);
        }
    }
}
