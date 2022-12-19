namespace Result;

public class Result<T>
{
    public T Value { get; protected set; }
    public Exception? Exception { get; protected set; }
    public bool IsSuccess => Exception == null;

    public static implicit operator bool(Result<T> result) => result.IsSuccess;
    public Result(T value) => Value = value;
    public Result(Exception error) => Exception = error;
}