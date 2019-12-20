using System;

namespace TagsCloudContainer.ResultInfrastructure
{
    public class Result
    {
        public string Error { get; }

        public Result(string error)
        {
            Error = error;
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null);
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

        public static Result<None> OfAction(Action f, string error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e.Message);
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


        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public bool IsSuccess => Error == null;

        public Result<TOutput> Then<TOutput>(Func<T, TOutput> continuation)
        {
            return Then(inp => Of(() => continuation(inp)));
        }

        public Result<None> Then(Action<T> continuation)
        {
            return Then(inp => OfAction(() => continuation(inp)));
        }

        public Result<TOutput> Then<TOutput>(Func<T, Result<TOutput>> continuation)
        {
            return IsSuccess
                ? continuation(Value)
                : Fail<TOutput>(Error);
        }

        public Result<T> OnFail(Action<string> handleError)
        {
            if (!IsSuccess) handleError(Error);
            return this;
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