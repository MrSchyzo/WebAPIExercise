using System;

namespace WebAPIExercise.Errors
{
    public class InvalidEntityException : Exception 
    {
        public InvalidEntityException(string message) : base(message) { }
    }
}
