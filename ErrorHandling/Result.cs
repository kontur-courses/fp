using System;

namespace ErrorHandling
{
    public struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public string Error { get; }
        public T Value { get; }
        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
        }

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<T> Unwrap<T>(this Result<Result<T>> result)
        {
            throw new NotImplementedException();
        }

        public static Result<TOutput> OnSuccess<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            throw new NotImplementedException();
        }

        public static Result<TOutput> OnSuccess<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            throw new NotImplementedException();
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            throw new NotImplementedException();
        }
    }
}