using System;
using System.Collections.Generic;
using System.Linq;

namespace Result
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
            Func<TInput, TOutput, TSelected> resultSelector)
        {
            return input.Then(continuation)
                        .Then(o => resultSelector(input.Value, o));
        }

        /// <summary>
        ///     Flattens sequence of <see cref="Result" /> to <see cref="Result" /> of sequence;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns>Successful <see cref="Result" /> if any of elements of sequence is successful. Otherwise returns Failure</returns>
        public static Result<IEnumerable<T>> AsResultSilently<T>(this IEnumerable<Result<T>> sequence)
        {
            var of = Result.Of(() => sequence.Where(r => r.IsSuccess)
                                             .Select(r => r.Value));
            return of;
        }

        /// <summary>
        ///     Flattens sequence of <see cref="Result" /> to <see cref="Result" /> of sequence;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns>Successful <see cref="Result" /> if all of elements of sequence is successful. Otherwise returns Failure</returns>
        public static Result<IEnumerable<T>> AsResult<T>(this IEnumerable<Result<T>> sequence)
        {
            var result = new List<T>();
            foreach (var element in sequence)
            {
                if (!element.IsSuccess)
                    return Result.Fail<IEnumerable<T>>(element.Error);
                result.Add(element.Value);
            }

            return ((IEnumerable<T>) result).AsResult();
        }
    }
}
