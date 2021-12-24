using System;
using JetBrains.Annotations;

namespace TagCloud
{
    [PublicAPI]
    public class Result<T> : Result
    {
        public Result(ResultErrorType? error, T value = default(T)) : base(error)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v) => Ok(v);

        public new ResultErrorType? Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess)
                return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }
    }

    [PublicAPI]
    public class Result
    {
        public ResultErrorType? Error { get; }

        public Result(ResultErrorType? error)
        {
            Error = error;
        }

        public bool IsSuccess => Error == null;

        public static Result<T> Ok<T>(T value) => new(null, value);


        public static Result<T> Fail<T>(ResultErrorType? e) => new(e);

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

        public static Result OfAction(Action f, ResultErrorType? error = null)
        {
            try
            {
                f();
                return new Result(null);
            }
            catch (Exception)
            {
                return new Result(error ?? ResultErrorType.DefaultError);
            }
        }
    }

    [PublicAPI]
    public static class ResultExtensions
    {
        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation
        ) => input.Then(inp => Result.Of(() => continuation(inp)));

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation
        ) => input.IsSuccess ? continuation(input.Value) : Result.Fail<TOutput>(input.Error);

        public static Result Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation
        ) => input.Then(inp => Result.OfAction(() => continuation(inp)));

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<ResultErrorType?> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }
    }
}