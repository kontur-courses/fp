namespace TagsCloudContainer.WordProcessing;

public class Result<T>
{
    public string? Error { get; }
    public T Value { get; }
    public bool IsSuccess => Error == null;

    public Result(string? error, T value = default(T))
    {
        Error = error;
        Value = value;
    }

    public T GetValueOrThrow()
    {
        if (IsSuccess) return Value;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }
}
