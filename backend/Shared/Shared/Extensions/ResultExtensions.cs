using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Errors;
using Shared.Common.Results;

namespace Shared.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToHttpResult(this Result result)
        {
            var body = GetOutputBody(result);

            if (result.IsSuccess)
                return new OkObjectResult(body);

            return result.Error switch
            {
                ConflictError => new ConflictObjectResult(body),

                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        public static IActionResult ToHttpResult<T>(this Result<T> result)
        {
            var body = GetOutputBody(result);

            if (result.IsSuccess)
                return new OkObjectResult(body);

            return result.Error switch
            {
                ConflictError => new ConflictObjectResult(body),

                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        private static object GetOutputBody(Result result)
        {
            return new
            {
                data = (object)null,
                error = result.Error?.Message
            };
        }

        private static object GetOutputBody<T>(Result<T> result)
        {
            return new
            {
                data = result.IsSuccess ? result.Data : (object)null,
                error = result.Error?.Message
            };
        }
    }
}
