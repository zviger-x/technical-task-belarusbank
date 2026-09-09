using Shared.Common.Errors;

namespace Products.Application.Common.Errors
{
    internal static class CategoryErrors
    {
        public static readonly Error CategoryToDeleteNotFound =
            new NotFoundError(
                "Category.Delete.NotFound",
                "Category is already deleted or not found.");

        public static readonly Error CategoryNotFound =
            new NotFoundError(
                "Category.NotFound",
                "Category not found.");
    }
}
