using System;

namespace TagCloudGenerator.ResultPattern
{
    public static class ResultExtensions
    {
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

        public static Result<TInput> ReplaceError<TInput>(this Result<TInput> inputResult,
                                                          Func<string, string> errorReplacer) =>
            inputResult.IsSuccess
                ? inputResult
                : Result.Fail<TInput>(errorReplacer(inputResult.Error));
    }
}