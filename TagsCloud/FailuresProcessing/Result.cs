using System;

namespace FailuresProcessing
{
    public struct Result<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess { get; }

        public Result(T value, string error, bool isSuccess)
        {
            Value = value;
            Error = !isSuccess && error == null ? 
                throw new ArgumentNullException(
                    $"{nameof(error)} should contain error message " +
                    $"when {nameof(isSuccess)} is false, but it was null") : 
                error;
            IsSuccess = isSuccess;
        }
    }
}