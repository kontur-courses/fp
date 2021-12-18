using System;

namespace ResultMonad
{
    public static class ResultQueryExpressionExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation) =>
            input.Then(continuation);

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation,
            Func<TInput, TOutput, TSelected> resultSelector) =>
            input
                .Then(continuation)
                .Then(o => resultSelector(input.Value, o));
    }
}