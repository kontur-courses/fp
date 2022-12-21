namespace TagCloudContainer.Result
{
    public struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }
        public static implicit operator Result<T>(T v)
        {
            return new Result<T>(null, v);
        }

        public string Error { get; }
        public T Value { get; }

        public bool IsSuccess => Error == null;
    }
}