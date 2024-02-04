namespace TagsCloud.Result;

public class None
{
    private None()
    {
    }
}

public struct Result<T>
{
    public Result(string error, T value = default(T))
    {
        Error = error;
        Value = value;
    }

    public string Error { get; }
    internal T Value { get; }

    public T GetValueOrThrow()
    {
        if (IsSuccess) return Value;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }

    public bool IsSuccess => Error == null;
}

public static class Result
{
    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static Result<None> Ok()
    {
        return Ok<None>(null);
    }

    public static Result<T> Fail<T>(string e)
    {
        return new Result<T>(e);
    }
}