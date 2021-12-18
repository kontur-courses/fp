using System.Collections.Generic;

namespace ResultMonad
{
    public static class ResultEnumerableExtensions
    {
        public static Result<IEnumerable<TInput>> Traverse<TInput>(this IEnumerable<Result<TInput>> collection)
        {
            var list = new List<TInput>();
            foreach (var value in collection)
            {
                if (!value.IsSuccess)
                    return Result.Fail<IEnumerable<TInput>>(value.Error);
                list.Add(value.Value);
            }

            return Result.Ok<IEnumerable<TInput>>(list);
        }
    }
}