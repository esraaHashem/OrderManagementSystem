using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrderManagementSystem.API
{
    /// <summary>
    /// Custom class to globally handle exception and return problem details info.
    /// </summary>
    public class AppCustomExceptionHandler : IExceptionHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            {
                var statusCode = exception is ArgumentException
                    ? StatusCodes.Status400BadRequest
                    : StatusCodes.Status500InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode == StatusCodes.Status400BadRequest ? "Bad Request" : "Internal Server Error",
                    Detail = exception?.Message,
                    Instance = httpContext.Request.Path
                };

                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

                return true;
            }
        }
    }
}