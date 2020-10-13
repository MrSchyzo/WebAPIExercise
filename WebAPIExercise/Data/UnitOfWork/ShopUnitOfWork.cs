using System;
using System.Threading.Tasks;

namespace WebAPIExercise.Data.UnitOfWork
{
    /// <summary>
    /// Functional version of a unit of work in which you can specify what you want to accomplish by executing a function inside a transaction
    /// </summary>
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

        /// <summary>
        /// <para>Asynchronously executes an async function inside a transaction block.</para>
        /// <para>The transaction block is automatically committed or rolled back depending on whether the function throws exception or not.</para>
        /// <para>This is obviously useless for reads and writes</para>
        /// </summary>
        /// <typeparam name="T">Type of the async function return value</typeparam>
        /// <param name="asyncBlock">An asynchronous function that has access to the UnitOfWork's instances of IProductRepository and IOrderRepository</param>
        /// <returns>The value returned by the function</returns>
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

        /// <summary>
        /// <para>Asynchronously executes a function inside a transaction block.</para>
        /// <para>The transaction block is automatically committed or rolled back depending on whether the function throws exception or not.</para>
        /// <para>This is obviously useless for reads and writes.</para>
        /// <para>
        /// The only difference between this overload and the other is that this receives a synchronous function as a method, leaving the 
        /// asynchrony to the infrastructural code
        /// </para>
        /// </summary>
        /// <typeparam name="T">Type of the async function return value</typeparam>
        /// <param name="block">A synchronous function that has access to the UnitOfWork's instances of IProductRepository and IOrderRepository</param>
        /// <returns>The value returned by the function</returns>
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
