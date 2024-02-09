namespace TagCloudResult
{
    public class Result
    {
        public bool IsSuccess => Error == null!;
        public string Error { get; }

        protected Result(string error)
        {
            Error = error;
        }

        public Result RefineError(string errorMessage) => ReplaceError(err => errorMessage + ". " + err);

        public Result ReplaceError(Func<string, string> replaceError) =>
            IsSuccess ? this : Fail(replaceError(Error));

        public static Result<T> Of<T>(Func<T> func, string error = null!) => Result<T>.Of(func, error);
        public static Result<T> Fail<T>(string error) => new(error);
        public static Result<T> Fail<T, TIn>(Result<TIn> result) => new(result.Error);
        public static Result Fail(string error) => new(error);
        public static Result Ok() => new(null!);
        public static Result<T> Ok<T>(T value) => new(null!, value);
    }

    public class Result<T> : Result
    {
        internal T Value { get; }
        public Result(string error, T value = default(T)) : base(error) => Value = value;

        public static implicit operator Result<T>(T v) => Ok(v);

        public T GetValueOrThrow() =>
            IsSuccess ? Value : throw new InvalidOperationException($"No value. Only Error {Error}");

        public static Result<T> Ok(T value) => new(null!, value);
        public new static Result<T> Fail(string e) => new(e);
        public Result<TOut> Fail<TOut>() => Result<TOut>.Fail(Error);
        public Result Fail() => Result.Fail(Error);

        public static Result<T> Of(Func<T> f, string? error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail(error ?? e.Message);
            }
        }

        public static Result OfAction(Action f, string? error = null)
        {
            try
            {
                f();
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(error ?? e.Message);
            }
        }

        public Result<TOutput> Then<TOutput>(Func<T, TOutput> continuation) =>
            Then(inp => Of(() => continuation(inp)));

        public Result Then(Action<T> continuation) =>
            Then(inp => OfAction(() => continuation(inp)));

        public Result<TOutput> Then<TOutput>(Func<T, Result<TOutput>> continuation) =>
            IsSuccess ? continuation(Value) : Result.Fail<TOutput>(Error);

        public Result<T> OnFail(
            Action<string> handleError)
        {
            if (!IsSuccess) handleError(Error);
            return this;
        }

        public new Result<T> ReplaceError(Func<string, string> replaceError) =>
            IsSuccess ? this : Fail(replaceError(Error));

        public new Result<T> RefineError(string errorMessage) =>
            ReplaceError(err => errorMessage + ". " + err);
    }
}
