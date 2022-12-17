using System;

namespace TagsCloud
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public string Error { get; }
        internal T Value { get; }
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error: {Error}");
        }
        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }
        public static Result<None> Ok()
        {
            return Ok<None>(null);
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

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            if (input.IsSuccess) return input;
            return Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + ". " + err);
        }
    }
}
