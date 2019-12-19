using System;

namespace TagCloudGenerator.ResultPattern
{
    public struct Result<T> : IResult
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

                throw new InvalidOperationException($"No value. Only error '{Error}'");
            }
        }

        public static explicit operator Result<T>(T value) => Result.Ok(value);
    }
}