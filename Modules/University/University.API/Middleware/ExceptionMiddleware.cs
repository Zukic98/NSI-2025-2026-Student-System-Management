using System.Net;
using System.Text.Json;
using University.Core.Exceptions;

namespace University.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";

                int statusCode;
                object response;

                if (ex is ValidationException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new { StatusCode = statusCode, Message = ex.Message };
                }
                else
                {
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    response = _env.IsDevelopment()
                        ? new { StatusCode = statusCode, Message = ex.Message, StackTrace = ex.StackTrace?.ToString() }
                        : new { StatusCode = statusCode, Message = "Internal Server Error", StackTrace = (string?)null };
                }

                context.Response.StatusCode = statusCode;

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
