using System;

namespace FunctionalTools
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        internal T Value { get; }

        internal Result(bool isSuccess, string error, T value = default(T))
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public T GetValueOrThrow()
        {
            if (IsSuccess)
                return Value;

            throw new InvalidOperationException($"There is no value, because of {Error}");
        }

        public static implicit operator Result<T>(T value)
        {
            return Result.Ok(value);
        }
    }

    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(true, null, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null);
        }

        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(false, error);
        }

        public static Result<T> Of<T>(Func<T> factory, string error = null)
        {
            try
            {
                return Ok(factory());
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
}