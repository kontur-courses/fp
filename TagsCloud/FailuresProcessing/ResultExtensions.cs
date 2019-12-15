using System;

namespace FailuresProcessing
{
    public static class Result
    {
        public static Result<T> Ok<T>(T value) => new Result<T>(value, null, true);
        public static Result<None> Ok() => Ok<None>(default);

        public static Result<T> Fail<T>(string error) => new Result<T>(default, error, false);

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

        public static Result<None> Of(Action action, string error = null)
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

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation) =>
                input.Then(v => Of(() => continuation(v)));

        public static Result<None> Then(
            this Result<None> input,
            Action continuation) =>
                input.Then(v => Of(continuation));

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation) =>
                input.Then(v => Of(() => continuation(v)));

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation) =>
                input.IsSuccess ?
                    continuation(input.Value) :
                    Fail<TOutput>(input.Error);

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError) =>
                input.IsSuccess ?
                    input :
                    Fail<TInput>(replaceError(input.Error));

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage) =>
                input.ReplaceError(err => $"{errorMessage}. {err}");
    }
}