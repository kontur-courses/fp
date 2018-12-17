using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
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
            throw new InvalidOperationException($"No value. Only Error {Error}");
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

        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(error);
        }

        public static Result<T> Of<T>(Func<T> func, string error = null)
        {
            try
            {
                return Ok(func());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<None> OfAction(Action action, string error = null)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e.Message);
            }
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.IsSuccess ? input : new Result<TInput>(errorMessage + " " + input.Error, input.Value);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.IsSuccess
                ? Ok(continuation(input.Value))
                : Fail<TOutput>(input.Error);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation()
                : Fail<TOutput>(input.Error);
        }

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action action, string error = null)
        {
            return input.IsSuccess ? OfAction(action, error) : Fail<None>(input.Error);
        }


        public static Result<TInput> Then<TInput>(
            this Result<TInput> input,
            Func<bool> continuation, string error)
        {
            if (input.IsSuccess)
                return continuation() ? Fail<TInput>(error) : input;
            return Fail<TInput>(input.Error);
        }

        public static Result<IEnumerable<TOutput>> Then<TOutput>(
            this Result<Dictionary<string, int>> input,
            Func<Dictionary<string, int>, IEnumerable<TOutput>> continuation, string error)
        {
            if (!input.IsSuccess)
                return Fail<IEnumerable<TOutput>>(input.Error);
            var result = Of(() => continuation(input.Value));
            if (!result.IsSuccess)
                return Fail<IEnumerable<TOutput>>(result.Error);
            return result.Value.Count() == input.Value.Count ? Ok(result.Value) : Fail<IEnumerable<TOutput>>(error);
        }
    }
}