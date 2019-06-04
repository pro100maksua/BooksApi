using System;
using System.Net;
using System.Threading.Tasks;
using Books.Logic.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Books.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case DuplicateBookException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                case BookNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result);
        }
    }
}
