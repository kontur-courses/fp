namespace TagCloudGenerator.ResultPattern
{
    public static class StringExtensions
    {
        public static Result<int> ParseToInt(this string numberAsString, string error = null) =>
            int.TryParse(numberAsString, out var number)
                ? number.AsResult()
                : Result.Fail<int>(error ?? $"Not a number '{numberAsString}'");
    }
}