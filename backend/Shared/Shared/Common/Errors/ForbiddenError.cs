namespace Shared.Common.Errors
{
    public class ForbiddenError : Error
    {
        public ForbiddenError(string code, string message)
            : base(code, message)
        {
        }
    }
}
