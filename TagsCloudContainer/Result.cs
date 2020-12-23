using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
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
            ErrorMessage = error;
            Value = value;
        }
        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public string ErrorMessage { get; }
        internal T Value { get; }
        public bool IsSuccess => ErrorMessage == null;
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

        public static Result<T> Fail<T>(string errorMessage)
        {
            return new Result<T>(errorMessage);
        }

        public static Result<T> Of<T>(Func<T> function, string errorMessage = null)
        {
            try
            {
                return Ok(function());
            }
            catch (Exception e)
            {
                return Fail<T>(errorMessage ?? e.Message);
            }
        }

        public static Result<None> OfAction(Action action, string errorMessage = null)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(errorMessage ?? e.Message);
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
                : Fail<TOutput>(input.ErrorMessage);
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            if (input.IsSuccess) return input;
            return Fail<TInput>(replaceError(input.ErrorMessage));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + ". " + err);
        }

        public static Result<IEnumerable<TInput>> EnumerateOrFail<TInput>(this IEnumerable<Result<TInput>> input)
        {
            string error = null;
            input = input.TakeWhile(t => (error = t.ErrorMessage) == null).ToArray();
            if (error != null) return Fail<IEnumerable<TInput>>(error);
            return input.Select(r => r.Value).AsResult();
        }
    }
}