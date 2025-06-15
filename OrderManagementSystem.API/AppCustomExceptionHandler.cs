using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrderManagementSystem.API
{
    /// <summary>
    /// Custom class to globally handle exception and return problem details info.
    /// </summary>
    public class AppCustomExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Create a proper problem details to have enough information for users.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = GetExceptioDetails(exception, httpContext);

            httpContext.Response.StatusCode = problemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

            return true;
        }

        private ProblemDetails GetExceptioDetails(Exception exception, HttpContext httpContext)
        {
            return exception switch
            {
                InvalidOrderStatusTransitionException => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Title = nameof(InvalidOrderStatusTransitionException),
                    Detail = exception?.Message,
                    Instance = httpContext.Request.Path
                },
                OrderNotFoundException => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = nameof(OrderNotFoundException),
                    Detail = exception?.Message,
                    Instance = httpContext.Request.Path
                },
                ArgumentException or ArgumentNullException => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                },

                _ => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "General Exception",
                    Detail = exception?.Message,
                    Instance = httpContext.Request.Path
                }
            };
        }
    }
}