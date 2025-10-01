using System.Net;

namespace BackendStore.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (ApplicationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode code, string? customMessage = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new
            {
                Success = false,
                Message = customMessage ?? ex.Message,
                Detail = ex is Exception ? ex.Message : null
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
