using System;

namespace TagCloudGenerator.ResultPattern
{
    public static class ResultExtensions
    {
        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> inputResult,
                                                            Func<TInput, Result<TOutput>> continuation) =>
            inputResult.IsSuccess
                ? continuation(inputResult.Value)
                : Result.Fail<TOutput>(inputResult.Error);

        public static Result<None> ActionOverValue<TInput>(this Result<TInput> inputResult,
                                                           Action<TInput> action) =>
            inputResult.Then(input => Result.OfAction(() => action(input)));

        public static Result<TOutput> SelectValue<TInput, TOutput>(this Result<TInput> inputResult,
                                                                   Func<TInput, TOutput> selector) =>
            inputResult.Then(input => Result.Of(() => selector(input)));

        public static Result<TInput> ReplaceError<TInput>(this Result<TInput> inputResult,
                                                          Func<string, string> errorReplacer) =>
            inputResult.IsSuccess
                ? inputResult
                : Result.Fail<TInput>(errorReplacer(inputResult.Error));
    }
}