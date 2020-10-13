using Functional.Maybe;
using System;

namespace WebAPIExercise.Utils
{
    /// <summary>
    /// Utility extensions for Maybe monad
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Either returns the wrapped value of the Maybe or throws the provided exception
        /// </summary>
        /// <typeparam name="T">Type of the value wrapped by the Maybe</typeparam>
        /// <typeparam name="E">Type of the provided exception</typeparam>
        /// <param name="maybe">This Maybe monad instance</param>
        /// <param name="exceptionSupplier">Function that supplies the exception</param>
        /// <returns>wrapped value of the Maybe</returns>
        public static T OrElseThrow<T, E>(this Maybe<T> maybe, Func<E> exceptionSupplier) where E : Exception
        {
            if (!maybe.HasValue)
            {
                throw exceptionSupplier.Invoke();
            }

            return maybe.Value;
        }
    }
}
