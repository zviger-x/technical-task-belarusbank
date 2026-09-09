using FluentValidation.Results;
using Shared.Common.Errors;

namespace Shared.Extensions
{
    public static class ValidationExtensions
    {
        public static ValidationError[] ToErrors(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .Select(x => new ValidationError(
                    $"Validation.{x.PropertyName}",
                    x.ErrorMessage))
                .ToArray();
        }
    }
}
