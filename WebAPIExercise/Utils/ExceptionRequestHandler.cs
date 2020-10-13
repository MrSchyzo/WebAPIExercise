using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPIExercise.Errors;

namespace WebAPIExercise.Utils
{
    public static class ExceptionRequestHandler
    {
        private static readonly Dictionary<Type, int> table = new Dictionary<Type, int>
        {
            [typeof(NotFoundException)] = 404,
            [typeof(InvalidEntityException)] = 400
        };

        public static RequestDelegate HandleExceptionInRequest = Handle;

        private static async Task Handle(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = table.GetValueOrDefault(exception.GetType(), 500);

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = exception.Message }));
        }
    }
}
