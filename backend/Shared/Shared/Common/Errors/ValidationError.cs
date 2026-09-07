namespace Shared.Common.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string code, string message)
            : base(code, message)
        {
        }
    }
}
