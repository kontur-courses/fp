namespace FunctionalStuff
{
    public static class Check
    {
        public static Result<string> StringIsEmpty(string value, string resultName) =>
            string.IsNullOrWhiteSpace(value)
                ? Result.Fail<string>($"{resultName} is empty")
                : Result.Ok(value);
    }
}