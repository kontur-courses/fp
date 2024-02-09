namespace TagCloudResult
{
    public class Result
    {
        public static Result Ok()
        {
            return new Result(null!);
        }

        protected Result(string error)
        {
            Error = error;
        }
        public static Result Fail(string error) => new(error);
        
        public bool IsSuccess => Error == null!;
        public string Error { get; }
    }

    public class Result<T>: Result
    {
        public Result(string error, T value = default(T)): base(error)
        {
            Value = value;
        }
        
        public static implicit operator Result<T>(T v)
        {
            return ResultIs.Ok(v);
        }
        
        
        internal T Value { get; }
        
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }
        
        
        public Result<T> Fail<T>()
        {
            return new Result<T>(Error);
        }
    }
}
