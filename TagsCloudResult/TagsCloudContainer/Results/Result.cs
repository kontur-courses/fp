using System;

namespace TagsCloudContainer.Results
{
    public class Result<T>
    {
        internal Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public bool IsSuccess => Error == null;
        public string Error { get; }
        private T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public static implicit operator Result<T>(T value) =>
            Result.Ok(value);
    }

    public static class Result
    {
        public static Result<T> Ok<T>(T value) => new(null, value);
        public static Result<None> Ok() => Ok<None>(null);

        public static Result<T> Fail<T>(string error) => new(error);
        public static Result<None> Fail(string error) => Fail<None>(error);

        public static Result<T> AsResult<T>(this T value) => Ok(value);

        public static Result<T> Of<T>(Func<T> func, string? error = null)
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

        public static Result<None> OfAction(Action action, string? error = null)
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
                ? continuation(input.GetValueOrThrow())
                : Fail<TOutput>(input.Error);
        }

        public static Result<None> Then(
            this Result<None> input,
            Func<Result<None>> continuation)
        {
            return input.Then(_ => continuation());
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            return input.IsSuccess
                ? input
                : Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + " " + err);
        }
    }
}