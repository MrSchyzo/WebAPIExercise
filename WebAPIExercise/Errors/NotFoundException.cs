using System;

namespace WebAPIExercise.Errors
{
    /// <summary>
    /// This exception represents an error caused by a not-found entity
    /// </summary>
    public class NotFoundException : Exception 
    {
        public NotFoundException(string message) : base(message) { }
    }
}
