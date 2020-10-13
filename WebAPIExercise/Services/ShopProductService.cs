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
    /// <summary>
    /// <inheritdoc cref="IProductService"/>
    /// <para>It uses a single DB UnitOfWork</para>
    /// </summary>
    public class ShopProductService : IProductService
    {
        private readonly ShopUnitOfWork unit;
        private readonly IMapper mapper;

        public ShopProductService(ShopUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        /// <summary>
        /// <inheritdoc cref="IProductService.GetAllPagedAsync(int, int)"/>
        /// <para>It does not check argument validity.</para>
        /// </summary>
        /// <param name="pageStart">0-based index of the page</param>
        /// <param name="pageSize">Size of the page</param>
        /// <returns>All Products in a chosen page</returns>
        public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageStart, int pageSize)
        {
            IEnumerable<DbProduct> products = await unit.ExecuteAsync(async (products, _) => await products.GetPage(pageStart, pageSize));

            return products.Select(mapper.Map<Product>);
        }

        /// <summary>
        /// <inheritdoc cref="IProductService.GetByIdAsync(int)"/>
        /// <para>It throws NotFoundException if the Product is not found</para>
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>A Product by id</returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            Maybe<DbProduct> maybe = await unit.ExecuteAsync(async (products, _) => await products.GetById(id));

            return maybe
                    .Select(mapper.Map<Product>)
                    .OrElseThrow(() => new NotFoundException($"Product {id} not found"));
        }

        /// <summary>
        /// <inheritdoc cref="IProductService.NewAsync(InOrder)"/>
        /// If there is already a Product that has same name and same description, an InvalidEntityException is thrown.
        /// </summary>
        /// <param name="product">POCO representing the Product to save</param>
        /// <returns>POCO representing the output Product</returns>
        public async Task<Product> NewAsync(InProduct product)
        {
            DbProduct newProduct = await unit.ExecuteAsync(async (products, _) =>
            {
                DbProduct toInsert = mapper.Map<DbProduct>(product);
                if (await products.IsThereAnyCollisionWith(toInsert))
                {
                    throw new InvalidEntityException("A product with same name and description already exists");
                }
                return await products.NewProduct(toInsert);
            });

            return mapper.Map<Product>(newProduct);
        }
    }
}
