namespace FunctionalStuff
{
    public readonly struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v) => Result.Ok(v);

        public string Error { get; }
        internal T Value { get; }

        public bool IsSuccess => Error == null;
    }
}