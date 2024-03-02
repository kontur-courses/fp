namespace ResultOf
{
    public enum ResultStatus
    {
        Ok,
        Error
    }

    public class Result<T>
    {
        public ResultStatus Status { get; private set; }
        public T Value { get; private set; }
        public string Error { get; private set; }

        private Result(ResultStatus status, T value, string errorMessage)
        {
            Status = status;
            Value = value;
            Error = errorMessage;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(ResultStatus.Ok, value, null);
        }

        public static Result<T> Fail(string errorMessage)
        {
            return new Result<T>(ResultStatus.Error, default(T), errorMessage);
        }

        public bool IsSuccess => Status == ResultStatus.Ok;
        public bool IsError => Status == ResultStatus.Error;

        public T GetValueOrDefault()
        {
            return IsSuccess ? Value : default(T);
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return IsSuccess ? Value : defaultValue;
        }

        public override string ToString()
        {
            return IsSuccess ? $"Result(Ok, {Value})" : $"Result(Error, {Error})";
        }
    }

    public static class ResultExtensions
    {
        public static Result<TResult> Then<T, TResult>(this Result<T> input, Func<T, Result<TResult>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Result<TResult>.Fail(input.Error);
        }

        public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Result<T>.Ok(f());
            }
            catch (Exception e)
            {
                return Result<T>.Fail(error ?? e.Message);
            }
        }
    }
}