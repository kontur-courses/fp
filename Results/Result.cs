namespace ResultOf;

public struct Result
{
    internal Result(string? error)
    {
        Error = error;
    }

    public string? Error { get; }
    public bool IsSuccess => Error == null;
}

public struct Result<T>
{
    internal Result(string? error, T? value = default)
    {
        Error = error;
        Value = value;
    }

    public static implicit operator Result<T>(T v)
    {
        return new(null, v);
    }

    public string? Error { get; }
    internal T? Value { get; }
    public T GetValueOrThrow()
    {
        if (IsSuccess)
            return Value!;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }
    public bool IsSuccess => Error == null;
}
