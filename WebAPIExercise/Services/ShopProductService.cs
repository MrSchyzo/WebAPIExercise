using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIExercise.Output;
using WebAPIExercise.Data.UnitOfWork;
using Functional.Maybe;
using WebAPIExercise.Utils;

using InProduct = WebAPIExercise.Input.Product;
using DbProduct = WebAPIExercise.Data.Models.Product;
using WebAPIExercise.Errors;

namespace WebAPIExercise.Services
{
    public class ShopProductService : IProductService
    {
        private readonly ShopUnitOfWork unit;
        private readonly IMapper mapper;

        public ShopProductService(ShopUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbProduct> products = await unit.ExecuteAsync(async (products, _) => await products.GetPage(pageStart, pageSize));

            return products.Select(mapper.Map<Product>);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Maybe<DbProduct> maybe = await unit.ExecuteAsync(async (products, _) => await products.GetById(id));

            return maybe
                    .Select(mapper.Map<Product>)
                    .OrElseThrow(() => new NotFoundException($"Product {id} not found"));
        }

        public async Task<Product> NewAsync(InProduct product)
        {
            DbProduct newProduct = await unit.ExecuteAsync(async (products, _) =>
            {
                DbProduct toInsert = mapper.Map<DbProduct>(product);
                if (await products.IsThereAnyCollisionsWith(toInsert))
                {
                    throw new InvalidEntityException("A product with same name and description already exists");
                }
                return await products.NewProduct(toInsert);
            });

            return mapper.Map<Product>(newProduct);
        }
    }
}
