using System;

namespace WebAPIExercise.Errors
{
    /// <summary>
    /// This exception represents an error caused by an invalid entity
    /// </summary>
    public class InvalidEntityException : Exception 
    {
        public InvalidEntityException(string message) : base(message) { }
    }
}
