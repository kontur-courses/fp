using TagsCloud.Extensions;

namespace TagsCloud.Results;

public readonly struct Result<T>
{
    public Result(string error, T value = default)
    {
        Error = error;
        Value = value;
    }

    public static implicit operator Result<T>(T v)
    {
        return ResultExtensions.Ok(v);
    }

    public string Error { get; }
    internal T Value { get; }

    public T GetValueOrThrow()
    {
        if (IsSuccess)
            return Value;

        throw new Exception(Error);
    }

    public bool IsSuccess => Error == null;
}