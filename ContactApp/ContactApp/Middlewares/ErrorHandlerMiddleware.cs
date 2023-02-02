using ContactApp.Models.ApiModels;
using Newtonsoft.Json;
using System.Net;

namespace ContactApp.Api.Middlewares
{
    internal class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string message = "Something went wrong";

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _ = error switch
                {
                    ArgumentException or InvalidOperationException => HandleServerErrorAsync(context, HttpStatusCode.BadRequest, error.Message),
                    KeyNotFoundException => HandleServerErrorAsync(context, HttpStatusCode.NotFound, error.Message),
                    UnauthorizedAccessException => HandleServerErrorAsync(context, HttpStatusCode.Unauthorized, error.Message),
                    _ => HandleServerErrorAsync(context, HttpStatusCode.InternalServerError, message)
                };
            }
        }

        private static Task HandleServerErrorAsync(HttpContext context, HttpStatusCode statusCode, string errorMessage)
        {
            // Logger should be added
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ContactApiResponse()
            {
                ErrorCode = (int)statusCode,
                Message = errorMessage
            }));
        }
    }
}
