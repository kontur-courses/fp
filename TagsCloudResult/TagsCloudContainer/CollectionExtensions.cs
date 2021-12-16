using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public static class CollectionExtensions
    {
        public static Result<IEnumerable<T>> CombineResults<T>(this IEnumerable<Result<T>> results)
        {
            var resultsList = results.ToList();
            var failed = resultsList.Find(r => !r.IsSuccess);
            return failed == null
                ? Result.Ok(resultsList.Select(r => r.GetValueOrThrow()))
                : Result.Fail<IEnumerable<T>>(failed.Error);
        }
    }
}