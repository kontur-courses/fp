using System.Collections;
using ErrorHandling;

namespace TagCloud.Extensions
{
    public static class EnumerableExtension
    {
        public static Result<None> ToOneResult(this IEnumerable enumerable)
        {
            foreach (var element in enumerable)
            {
                if (element is Result<None> result)
                    if (!result.IsSuccess)
                        return Result.Fail<None>(result.Error);
            }

            return Result.Ok<None>(null);
        }
    }
}