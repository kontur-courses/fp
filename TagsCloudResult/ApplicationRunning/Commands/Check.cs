using System.Linq;

namespace TagsCloudResult.ApplicationRunning.Commands
{
    public static class Check
    {
        public static Result<T> Argument<T>(T argument, params bool[] conditions)
        {
            var ok = conditions.All(c => c);
            return ok ? Result.Ok(argument) : Result.Fail<T>($"Not a valid argument {argument}");
        }

        public static Result<string[]> ArgumentsCountIs(string[] arguments, int expectedCount)
        {
            var ok = arguments.Length == expectedCount;
            return ok
                ? Result.Ok(arguments)
                : Result.Fail<string[]>($"Incorrect arguments count! Expected {expectedCount}.");
        }

        public static Result<T> Argument<T>(T argument, string message, params bool[] conditions)
        {
            var ok = conditions.All(c => c);
            return ok ? Result.Ok(argument) : Result.Fail<T>(message);
        }
    }
}