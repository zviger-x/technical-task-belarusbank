using Shared.Common.Errors;

namespace Shared.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);

        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        private Result(bool isSuccess, T value, Error error)
            : base(isSuccess, error)
        {
            Data = value;
        }

        public static Result<T> Success(T value)
            => new(true, value, null);

        public static new Result<T> Failure(Error error)
            => new(false, default, error);
    }
}
