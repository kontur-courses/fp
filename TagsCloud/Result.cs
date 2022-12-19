using System;

namespace TagsCloud
{
    public class None
    {
        private None()
        {
        }
    }

    public class Result<T>
    {
        public string Error { get; }
        public T Value { get; }
        public bool IsSuccess => Error == null;

        private Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error: {Error}");
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
    }
}
