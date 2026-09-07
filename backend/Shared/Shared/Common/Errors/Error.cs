namespace Shared.Common.Errors
{
    public abstract class Error
    {
        public string Code { get; }
        public string Message { get; }

        protected Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
