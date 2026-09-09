using Shared.Common.Errors;

namespace Shared.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error[] Errors { get; }

        protected Result(bool isSuccess, Error[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new(true, Array.Empty<Error>());

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);

        public static Result Failure(Error error) => new(false, [error]);

        public static Result Failure(Error[] errors) => new(false, errors);

        public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

        public static Result<T> Failure<T>(Error[] errors) => Result<T>.Failure(errors);
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        private Result(bool isSuccess, T value, Error[] errors)
            : base(isSuccess, errors)
        {
            Data = value;
        }

        public static Result<T> Success(T value) => new(true, value, Array.Empty<Error>());

        public static new Result<T> Failure(Error error) => new(false, default, [error]);

        public static new Result<T> Failure(Error[] errors) => new(false, default, errors);
    }
}
