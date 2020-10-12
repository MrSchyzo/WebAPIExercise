using Functional.Maybe;
using System;

namespace WebAPIExercise.Utils
{
    public static class MaybeExtensions
    {
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
