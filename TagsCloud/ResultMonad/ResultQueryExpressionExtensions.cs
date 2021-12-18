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

    public static class ResultNoneExtensions
    {
        public static Result<None> ToNone<TInput>(this Result<TInput> input) => input.Then(_ => Result.Ok());

        public static Result<TOutput> Then<TOutput>(this Result<None> input, Func<Result<TOutput>> continuation) =>
            input.Then(_ => continuation());

        public static Result<TOutput> Then<TOutput>(this Result<None> input, Func<TOutput> continuation) =>
            input.Then(_ => continuation());
    }
}