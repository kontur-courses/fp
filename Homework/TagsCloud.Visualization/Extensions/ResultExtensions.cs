using System;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.Extensions
{
    public static class ResultExtensions
    {
        public static Result<T> Validate<T>(
            this Result<T> input,
            Predicate<T> validator,
            Func<T, string> errorMessage)
        {
            if (input.IsSuccess)
                return validator(input.Value)
                    ? input
                    : Result.Fail<T>(errorMessage(input.Value));
            return input;
        }

        public static Result<T> Validate<T>(this Result<T> input, Predicate<T> validator, string errorMessage)
            => input.Validate(validator, _ => errorMessage);

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Result.Of(() => continuation(inp)));
        }

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => Result.Of(() => continuation(inp)));
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation) =>
            input.IsSuccess
                ? continuation(input.Value)
                : Result.Fail<TOutput>(input.Error);

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }
    }
}