using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPIExercise.Errors;

namespace WebAPIExercise.Utils
{
    /// <summary>
    /// Custom RequestDelegate used to transform exceptions into custom JSON for API response
    /// </summary>
    public static class ExceptionRequestHandler
    {
        private static readonly Dictionary<Type, int> errorTypeToStatusCode = new Dictionary<Type, int>
        {
            [typeof(NotFoundException)] = 404,
            [typeof(InvalidEntityException)] = 400
        };

        /// <summary>
        /// Transforms exceptions into custom JSON for API response
        /// </summary>
        public static RequestDelegate HandleExceptionInRequest = Handle;

        private static async Task Handle(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorTypeToStatusCode.GetValueOrDefault(exception.GetType(), 500);

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = exception.Message }));
        }
    }
}
