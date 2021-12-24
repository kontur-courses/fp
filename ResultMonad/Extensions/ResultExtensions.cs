using System;

namespace ResultMonad.Extensions
{
    public static class ResultExtensions
    {
        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> continuation) =>
            input.Then(inp => Result.Of(() => continuation(inp)));

        public static Result<TOutput> Then<TOutput>(this Result<None> input, Func<TOutput> continuation) =>
            input.Then(_ => continuation());

        public static Result<None> Then<TInput, TOutput>(this Result<TInput> input, Action<TInput> continuation) =>
            input.Then(inp => OfAction(() => continuation(inp)));

        public static Result<None> Then<TInput>(this Result<TInput> input, Action<TInput> continuation) =>
            input.Then(inp => OfAction(() => continuation(inp)));

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, Result<TOutput>> continuation) =>
            input.IsSuccess
                ? continuation(input.Value)
                : Result.Fail<TOutput>(input.Error);

        public static Result<TInput> OnFail<TInput>(this Result<TInput> input, Action<string> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(this Result<TInput> input, Func<string, string> replaceError) =>
            input.IsSuccess ? input : Result.Fail<TInput>(replaceError(input.Error));

        public static Result<TInput> RefineError<TInput>(this Result<TInput> input, string errorMessage) =>
            input.ReplaceError(err => errorMessage + ". " + err);

        public static Result<TInput> Validate<TInput>(this Result<TInput> input, Func<TInput, bool> validator,
            string errorMessage) =>
            input.Then(i => validator(i) ? Result.Ok(i) : Result.Fail<TInput>(errorMessage));

        public static Result<TInput> Validate<TInput>(this Result<TInput> input, bool condition,
            string errorMessage) =>
            input.Then(i => condition ? Result.Ok(i) : Result.Fail<TInput>(errorMessage));

        public static Result<None> OfAction(Action f, string error = null)
        {
            try
            {
                f();
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail<None>(error ?? e.Message);
            }
        }
    }
}