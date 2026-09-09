using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Common.Errors;
using Shared.Common.Results;
using Shared.Extensions;

namespace Shared.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private const string UnhandledErrorMessage = "Unhandled exception occurred while processing the request.";

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UnhandledErrorMessage);

                var result = Result.Failure(new InternalError("InternalError", UnhandledErrorMessage));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(ResultExtensions.GetOutputBody(result));
            }
        }
    }
}
