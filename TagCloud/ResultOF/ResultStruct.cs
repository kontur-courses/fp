using System;

namespace ResultOF
{
    public struct Result<T>
    {
        public string Error { get; }
        public T Value { get; }

        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }
        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"Error occured: {Error}");
        }

        public bool IsSuccess => Error == null;
    }
}
