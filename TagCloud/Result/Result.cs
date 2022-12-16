namespace Result;

public class Result<T>
{
    public string? Error { get; }
    public T? Value { get; }
    public bool IsSuccess => Error is null;

    public Result(T? value, string? error = null)
    {
        Error = error;
        Value = value;
    }

    public static implicit operator T?(Result<T?> result) => result.Value;
}