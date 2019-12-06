using System;

namespace ResultOfTask
{
    public static class ResultQueryExpressionExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess ? 
                continuation(input.Value) : 
                new Result<TOutput>(input.Error);
        }

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation,
            Func<TInput, TOutput, TSelected> resultSelector)
        {
            if (!input.IsSuccess)
                return new Result<TSelected>(input.Error);

            var t = input.SelectMany(continuation);
            return t.IsSuccess ? 
                new Result<TSelected>(null, resultSelector(input.Value, t.Value)) : 
                new Result<TSelected>(t.Error);
        }
    }
}