using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;

namespace FunctionalStuff.Common
{
    public static class EnumerableExtensions
    {
        public static Result<ICollection<T>> ToResult<T>(this IEnumerable<Result<T>> source)
        {
            var arr = source.ToArray();

            var failed = arr.Where(x => !x.IsSuccessful).Select(x => x.Error).ToArray();
            if (failed.Length > 0)
                return Result.Fail<ICollection<T>>(failed.JoinStrings(", "));

            ICollection<T> successfulResults = arr.Select(x => x.Value).ToArray();
            return Result.Ok(successfulResults);
        }

        public static string ToStringWith<T>(this IEnumerable<T> source, string separator, Func<T, string> mapper) =>
            source.Select(mapper).JoinStrings(separator);

        public static string JoinStrings(this IEnumerable<string> source, string separator) =>
            string.Join(separator, source);
    }
}