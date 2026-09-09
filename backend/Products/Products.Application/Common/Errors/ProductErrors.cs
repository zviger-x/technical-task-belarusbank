using Shared.Common.Errors;

namespace Products.Application.Common.Errors
{
    internal static class ProductErrors
    {
        public static readonly Error ProductToDeleteNotFound =
            new NotFoundError(
                "Product.Delete.NotFound",
                "Product is already deleted or not found.");

        public static readonly Error ProductNotFound =
            new NotFoundError(
                "Product.NotFound",
                "Product not found.");
    }
}
