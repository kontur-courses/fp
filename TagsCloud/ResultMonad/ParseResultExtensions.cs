using System;

namespace ResultMonad
{
    public static class ParseResultExtensions
    {
        public static Result<int> ParseIntResult(this string s, string error = null) =>
            int.TryParse(s, out var v)
                ? v.AsResult()
                : Result.Fail<int>(error ?? "Не число " + s);

        public static Result<Guid> ParseGuidResult(this string s, string error = null) =>
            Guid.TryParse(s, out var v)
                ? v.AsResult()
                : Result.Fail<Guid>(error ?? "Не GUID " + s);
    }
}