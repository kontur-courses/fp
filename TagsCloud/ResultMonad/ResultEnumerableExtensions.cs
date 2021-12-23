using System.Collections.Generic;

namespace ResultMonad
{
    public static class ResultEnumerableExtensions
    {
        public static Result<IEnumerable<TInput>> Traverse<TInput>(this IEnumerable<Result<TInput>> source)
        {
            return Result.Ok()
                .Validate(() => source is not null, $"{nameof(source)} was null")
                .Then(() =>
                {
                    var results = new List<TInput>();
                    foreach (var element in source)
                    {
                        if (!element.IsSuccess)
                            return Result.Fail<IEnumerable<TInput>>(element.Error);
                        results.Add(element.Value);
                    }

                    return results;
                });
        }
    }
}