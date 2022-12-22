namespace Result;

public readonly struct Result<T>
{
    public T Value { get; }
    public Exception? Exception { get; }
    public bool IsSuccess => Exception == null;

    public static implicit operator bool(Result<T> result) => result.IsSuccess;

    public Result(T value)
    {
        Value = value;
        Exception = null;
    }

    public Result(Exception error)
    {
        Value = default;
        Exception = error;
    }
}