using System;

namespace TagCloudGenerator.ResultPattern
{
    public struct Result<T>
    {
        private readonly T value;

        public Result(string error, T value = default)
        {
            Error = error;
            this.value = value;
        }

        public string Error { get; }
        public bool IsSuccess => Error is null;

        public T Value
        {
            get
            {
                if (IsSuccess)
                    return value;

                throw new InvalidOperationException($"No value. Only Error {Error}");
            }
        }

        public static implicit operator Result<T>(T value) => Result.Ok(value);
    }
}