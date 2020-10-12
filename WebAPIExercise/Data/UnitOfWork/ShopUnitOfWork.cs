using System;
using System.Threading.Tasks;

namespace WebAPIExercise.Data.UnitOfWork
{
    public class ShopUnitOfWork
    {
        private readonly ShopContext shopContext;
        private readonly IProductRepository products;
        private readonly IOrderRepository orders;

        public ShopUnitOfWork(ShopContext shopContext, IProductRepository products, IOrderRepository orders)
        {
            this.shopContext = shopContext;
            this.products = products;
            this.orders = orders;
        }

        public async Task<T> ExecuteAsync<T>(Func<IProductRepository, IOrderRepository, Task<T>> asyncBlock)
        {
            using var transaction = await shopContext.Database.BeginTransactionAsync();
            try
            {
                T result = await asyncBlock.Invoke(products, orders);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<IProductRepository, IOrderRepository, T> block)
        {
            using var transaction = await shopContext.Database.BeginTransactionAsync();
            try
            {
                T result = block.Invoke(products, orders);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
        }
    }
}
