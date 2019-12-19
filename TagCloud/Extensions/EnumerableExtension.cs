using System.Collections.Generic;
using ErrorHandling;

namespace TagCloud.Extensions
{
    public static class EnumerableExtension
    {
        public static Result<None> ToOneResult(this IEnumerable<Result<None>> enumerable)
        {
            var isFirstFail = true;
            var allUniqueErrors = new HashSet<string>();
            var result = Result.Ok<None>(null);
            foreach (var element in enumerable)
            {
                if (element is Result<None> currentResult)
                    if (!currentResult.IsSuccess)
                    {
                        if (isFirstFail)
                        {
                            isFirstFail = false;
                            result = Result.Fail<None>(currentResult.Error);
                        }
                        else if (!allUniqueErrors.Contains(currentResult.Error))
                            result = result.ReplaceError(x => x + "\n" + currentResult.Error);
                        allUniqueErrors.Add(currentResult.Error);
                    }
            }

            return result;
        }
    }
}