using System;

namespace TagCloudContainer.ResultMonad
{
    public struct Result<T>
    {
        public Result(string error = default, T value = default)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public string Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public T DefaultIfError()
        {
            return IsSuccess ? Value : default;
        }

        public T DefaultIfError(T defaultValue)
        {
            return IsSuccess ? Value : defaultValue;
        }

        public bool IsSuccess => Error == null;
    }
}