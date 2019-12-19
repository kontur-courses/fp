using System;

namespace ErrorHandling
{
    public static class ResultQueryExpressionExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.Then(continuation);
        }
    }
}