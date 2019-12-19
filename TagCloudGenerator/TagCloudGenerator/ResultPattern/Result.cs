using System;

namespace TagCloudGenerator.ResultPattern
{
    public static class Result
    {
        public static Result<T> Ok<T>(T value) => new Result<T>(null, value);

        private static Result<None> Ok() => Ok<None>(null);

        public static Result<T> AsResult<T>(this T value) => Ok(value);

        public static Result<T> Fail<T>(string error) => new Result<T>(error);

        public static Result<T> Of<T>(Func<T> function, string error = null)
        {
            try
            {
                return Ok(function());
            }
            catch (Exception exception)
            {
                return Fail<T>(error ?? exception.Message);
            }
        }

        public static Result<None> OfAction(Action action, string error = null)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception exception)
            {
                return Fail<None>(error ?? exception.Message);
            }
        }

        public static IResult FindErrorResult(params IResult[] results)
        {
            foreach (var result in results)
                if (!result.IsSuccess)
                    return result;
            return Ok();
        }
    }
}