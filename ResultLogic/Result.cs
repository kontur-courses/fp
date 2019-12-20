using System;

namespace ResultLogic
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(Exception error, T value = default)
        {
            Error = error;
            Value = value;
        }
        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public Exception Error { get; }
        public T Value { get; }
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }
        public bool IsSuccess => Error == null;


        //public static implicit operator Result<T>(T value)
        //{
        //    return Result.Ok(value);
        //}
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

        public static Result<T> Fail<T>(Exception exception)
        {
            return new Result<T>(exception);
        }

        public static Result<T> Of<T>(Func<T> f, Exception error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e);
            }
        }

        public static Result<None> OfAction(Action f, Exception error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<None> Then<TInput, TOutput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
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
            Action<Exception> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }
    }
}