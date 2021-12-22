using System;

namespace TagCloud
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(ResultErrorType? error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public ResultErrorType? Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null);
        }

        public static Result<T> Fail<T>(ResultErrorType? e)
        {
            return new Result<T>(e);
        }

        public static Result<T> Of<T>(Func<T> f, ResultErrorType? error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception)
            {
                return Fail<T>(error ?? ResultErrorType.DefaultError);
            }
        }

        public static Result<None> OfAction(Action f, ResultErrorType? error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception)
            {
                return Fail<None>(error ?? ResultErrorType.DefaultError);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }
        
        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<ResultErrorType?> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }
    }
}