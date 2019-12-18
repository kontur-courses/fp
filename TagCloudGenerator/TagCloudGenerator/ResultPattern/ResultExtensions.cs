using System;

namespace TagCloudGenerator.ResultPattern
{
    public static class ResultExtensions
    {
        public static Result<T> AsResult<T>(this T value) => Result.Ok(value);

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> inputResult,
                                                            Func<TInput, TOutput> continuation) =>
            inputResult.Then(input => Result.Of(() => continuation(input)));

        public static Result<None> Then<TInput>(this Result<TInput> inputResult,
                                                Action<TInput> continuation) =>
            inputResult.Then(input => Result.OfAction(() => continuation(input)));

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> inputResult,
                                                            Func<TInput, Result<TOutput>> continuation) =>
            inputResult.IsSuccess
                ? continuation(inputResult.Value)
                : Result.Fail<TOutput>(inputResult.Error);

        public static Result<TInput> OnFail<TInput>(this Result<TInput> inputResult,
                                                    Action<string> errorHandler)
        {
            if (!inputResult.IsSuccess)
                errorHandler(inputResult.Error);

            return inputResult;
        }

        public static Result<TInput> ReplaceError<TInput>(this Result<TInput> inputResult,
                                                          Func<string, string> errorReplacer) =>
            inputResult.IsSuccess
                ? inputResult
                : Result.Fail<TInput>(errorReplacer(inputResult.Error));

        public static Result<TInput> RefineError<TInput>(this Result<TInput> inputResult,
                                                         string errorMessage) =>
            inputResult.ReplaceError(error => $"{errorMessage}. {error}");
    }
}