using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Errors;
using Shared.Common.Results;

namespace Shared.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToHttpResult(this Result result) => ToHttpResult(result, GetOutputBody(result));

        public static IActionResult ToHttpResult<T>(this Result<T> result) => ToHttpResult(result, GetOutputBody(result));

        private static IActionResult ToHttpResult(Result result, object body)
        {
            if (result.IsSuccess)
                return new OkObjectResult(body);

            return result.Errors[0] switch
            {
                ConflictError => new ConflictObjectResult(body),
                ValidationError => new BadRequestObjectResult(body),
                NotFoundError => new NotFoundObjectResult(body),

                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        private static object GetOutputBody(Result result)
        {
            return new
            {
                data = (object)null,
                errors = result.Errors
            };
        }

        private static object GetOutputBody<T>(Result<T> result)
        {
            return new
            {
                data = result.IsSuccess ? result.Data : (object)null,
                errors = result.Errors
            };
        }
    }
}
