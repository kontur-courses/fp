using System.Collections.Generic;
using ErrorHandling;

namespace TagCloud.Extensions
{
    public static class EnumerableExtension
    {
        public static Result<None> ToOneResult(this IEnumerable<Result<None>> enumerable)
        {
            var uniqueErrors = new HashSet<string>();
            foreach (var currentResult in enumerable)
                if (!currentResult.IsSuccess)
                    if (!uniqueErrors.Contains(currentResult.Error))
                        uniqueErrors.Add(currentResult.Error);

            return uniqueErrors.Count == 0
                ? Result.Ok<None>(null)
                : Result.Fail<None>(string.Join(";", uniqueErrors));
        }
    }
}