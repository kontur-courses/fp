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

        internal object Then<T1>()
        {
            throw new NotImplementedException();
        }

        public Result<TResult> Then<TResult>(Func<T, TResult> onSuccess)
        {
            if (IsSuccess)
            {
                return Result<TResult>.Ok(onSuccess(Value));
            }
            else
            {
                return Result<TResult>.Fail(Error);
            }
        }
    }
}