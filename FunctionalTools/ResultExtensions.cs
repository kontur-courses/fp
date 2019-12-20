using System;

namespace FunctionalTools
{
    public static class ResultExtensions
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Result.Ok(value);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Result.Fail<TOutput>(input.Error);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Result.Of(() => continuation(inp)));
        }

        public static Result<T> OnFail<T>(
            this Result<T> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);

            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            return input.IsSuccess ? input : Result.Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(previousError => $"{errorMessage}. {previousError}");
        }
    }
}