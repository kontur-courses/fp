namespace TagsCloudContainer;

public class Result
{
    public Result(string error)
    {
        Error = error;
    }

    public string Error { get; }

    public bool IsSuccess => Error == null;

    public static Result Ok()
    {
        return new Result(null);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static Result Fail(string e)
    {
        return new Result(e);
    }

    public static Result<T> Fail<T>(string e)
    {
        return new Result<T>(e);
    }

    public static Result OfAction(Action f, string error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail(error ?? e.Message);
        }
    }
}

public class Result<T> : Result
{
    public Result(string error, T value = default(T)) : base(error)
    {
        Value = value;
    }

    public T Value { get; }

    public T GetValueOrThrow()
    {
        if (IsSuccess) return Value;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }

    public static implicit operator Result<T>(T value)
    {
        return Ok(value);
    }

    public static Result<T> Of(Func<T> f, string error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? e.Message);
        }
    }
}