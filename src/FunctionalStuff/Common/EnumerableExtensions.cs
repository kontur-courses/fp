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
            var list = new List<T>();
            var errors = new List<string>();
            foreach (var r in source)
            {
                if (!r.IsSuccessful) errors.Add(r.Error);
                else if (errors.Count == 0) list.Add(r.Value);
            }

            return errors.Count == 0
                ? Result.Ok<ICollection<T>>(list)
                : Result.Fail<ICollection<T>>(string.Join(", ", errors));
        }

        public static string ToStringWith<T>(this IEnumerable<T> source, string separator, Func<T, string> mapper) =>
            source.Select(mapper).JoinStrings(separator);

        public static string JoinStrings(this IEnumerable<string> source, string separator) =>
            string.Join(separator, source);
    }
}