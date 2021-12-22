using System;

namespace TagsCloud.Visualization.Utils
{
    public readonly struct Result<T>
    {
        internal Result(T value, string error)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T value) => Result.Ok(value);

        public string Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value) => Ok(value);

        public static Result<T> Ok<T>(T value) => new(value, null);

        public static Result<None> Ok() => Ok<None>(null);

        public static Result<T> Fail<T>(string error) => new(default, error);

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

        public static Result<None> Of(Action f, string error = null)
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
}