using System.Collections.Generic;
using FunctionalStuff.Results;

namespace FunctionalStuff.General
{
    public static class EnumerableExtensions
    {
        public static Result<ICollection<T>> ToResult<T>(this IEnumerable<Result<T>> enumerable)
        {
            var list = new List<T>();
            var errors = new List<string>();
            foreach (var r in enumerable)
            {
                if (!r.IsSuccess) errors.Add(r.Error);
                else if (errors.Count == 0) list.Add(r.Value);
            }

            return errors.Count == 0
                ? Result.Ok<ICollection<T>>(list)
                : Result.Fail<ICollection<T>>(string.Join(", ", errors));
        }
    }
}