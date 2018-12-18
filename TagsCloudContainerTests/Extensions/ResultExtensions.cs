using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer;

namespace TagsCloudContainerTests.Extensions
{
    public static class ResultExtensions
    {
        public static IEnumerable<T> GetValues<T>(this IEnumerable<Result<T>> values)
            => values.Select(x => x.GetValueOrThrow());
    }
}