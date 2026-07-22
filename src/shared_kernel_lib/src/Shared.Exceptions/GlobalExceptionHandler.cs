using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Shared.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        public GlobalExceptionHandler(IHostEnvironment env)
        {
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var caption = "Lỗi hệ thống";
            var message = "Đã có lỗi xảy ra. Vui lòng thử lại sau.";

            if (exception is BaseException baseException)
            {
                statusCode = baseException.StatusCode;
                caption = baseException.Code; // Ví dụ: "NOT_FOUND"
                message = baseException.Message;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Success = false,
                Caption = caption,
                Message = message,
                Data = _env.IsDevelopment() ? new { Detail = exception.ToString() } : null
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await httpContext.Response.WriteAsJsonAsync(errorResponse, jsonOptions, cancellationToken);

            return true;
        }
    }
}