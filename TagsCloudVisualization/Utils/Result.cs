using System;

namespace TagsCloudVisualization.Utils
{
    public class Result
    {
        public Result(string error = null)
        {
            Error = error;
        }

        public string Error { get; }

        public bool IsSuccess => Error == null;

        public static Result Ok()
        {
            return new Result();
        }

        public static Result Fail(string error)
        {
            return new Result(error);
        }

        public static Result OfAction(Action f, string error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(error ?? e.Message);
            }
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
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

        public Result ReplaceError(Func<string, string> replaceError)
        {
            if (IsSuccess) return this;
            return Fail(replaceError(Error));
        }

        public Result RefineError(string errorMessage)
        {
            return ReplaceError(err => errorMessage + ". " + err);
        }
    }

    public class Result<T> : Result
    {
        public Result(string error, T value = default(T)) : base(error)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Ok(v);
        }

        internal T Value { get; }
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public Result<TOutput> Then<TOutput>(Func<T, TOutput> continuation)
        {
            return Then(inp => Of(() => continuation(Value)));
        }

        public Result Then(Action<T> continuation)
        {
            return Then(inp => OfAction(() => continuation(Value)));
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

        public new Result<T> ReplaceError(Func<string, string> replaceError)
        {
            if (IsSuccess) return this;
            return Fail<T>(replaceError(Error));
        }

        public new Result<T> RefineError(string errorMessage)
        {
            return ReplaceError(err => errorMessage + ". " + err);
        }
    }
}