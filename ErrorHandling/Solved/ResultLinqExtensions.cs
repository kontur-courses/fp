using System;

namespace ErrorHandling.Solved
{
    public static class ResultLinqExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.OnSuccess(continuation);
        }

        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.OnSuccess(continuation);
        }

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation,
            Func<TInput, TOutput, TSelected> resultSelector)
        {
            return input
                .OnSuccess(continuation)
                .OnSuccess(o => resultSelector(input.Value, o));
        }

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation,
            Func<TInput, Result<TOutput>, Result<TSelected>> resultSelector)
        {
            var output = input.OnSuccess(continuation);
            return output.OnSuccess(o => resultSelector(input.Value, output));
        }
    }
}