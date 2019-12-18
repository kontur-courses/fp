namespace TagCloudGenerator.ResultPattern
{
    public static class ParseResultExtensions
    {
        public static Result<int> ParseIntResult(this string s, string error = null) =>
            int.TryParse(s, out var v)
                ? v.AsResult()
                : Result.Fail<int>(error ?? "Not a number" + s);
    }
}