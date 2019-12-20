using System;

namespace TagsCloudContainer.ResultInfrastructure
{
    public class Result
    {
        public string Error { get; }

        public bool IsSuccess => Error == null;

        public Result(string error)
        {
            Error = error;
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result Ok()
        {
            return new Result(null);
        }

        public static Result Fail(string error)
        {
            return new Result(error);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
        }

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }
    }

    public class Result<T> : Result
    {
        internal T Value { get; }

        public Result(string error, T value = default(T)) : base(error)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }


        public Result<TOutput> Then<TOutput>(Func<T, TOutput> continuation)
        {
            return Then(inp => Of(() => continuation(inp)));
        }

        public Result<TOutput> Then<TOutput>(Func<T, Result<TOutput>> continuation)
        {
            return IsSuccess
                ? continuation(Value)
                : Fail<TOutput>(Error);
        }

        public Result<T> ReplaceError(Func<string, string> replaceError)
        {
            return IsSuccess ? this : Fail<T>(replaceError(Error));
        }

        public Result<T> RefineError(string errorMessage)
        {
            return ReplaceError(err => errorMessage + ". " + err);
        }
    }
}