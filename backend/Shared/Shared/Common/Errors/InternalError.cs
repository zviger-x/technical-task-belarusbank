namespace Shared.Common.Errors
{
    public class InternalError : Error
    {
        public InternalError(string code, string message)
            : base(code, message)
        {
        }
    }
}
