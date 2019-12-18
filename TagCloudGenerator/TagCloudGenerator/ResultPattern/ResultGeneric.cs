using System;

namespace TagCloudGenerator.ResultPattern
{
    public struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public string Error { get; }
        public bool IsSuccess => Error == null;
        internal T Value { get; }

        public static implicit operator Result<T>(T v) => Result.Ok(v);

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }
    }
}