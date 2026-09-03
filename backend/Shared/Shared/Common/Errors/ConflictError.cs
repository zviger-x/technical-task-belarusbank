namespace Shared.Common.Errors
{
    public class ConflictError : Error
    {
        public ConflictError(string code, string message)
            : base(code, message)
        {
        }
    }
}
