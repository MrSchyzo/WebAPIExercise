using System;

namespace WebAPIExercise.Errors
{
    public class NotFoundException : Exception 
    {
        public NotFoundException(string message) : base(message) { }
    }
}
