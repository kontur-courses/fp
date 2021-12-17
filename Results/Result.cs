using ResultExtensions;

namespace ResultOf;

public struct Result
{
    internal Result(string? error)
    {
        Error = error!;
    }

    public string Error { get; }
    public bool IsSuccess => Error == null;

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }
    public static Result Ok()
    {
        return new(null);
    }

    public static Result Fail(string e)
    {
        return new(e);
    }

    public static Result<T> Fail<T>(string e)
    {
        return new Result<T>(e);
    }

    public static Result<T> Of<T>(Func<T> f, string? error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            var fail = Fail<T>(error ?? e.Message);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                fail = fail.RefineError(e.Message);
            }

            return fail;
        }
    }

    public static Result OfAction(Action f, string? error = null)
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

public struct Result<T>
{
    internal Result(string? error, T? value = default)
    {
        Error = error!;
        Value = value;
    }

    public string Error { get; }

    public static implicit operator Result<T>(T v)
    {
        return new(null, v);
    }
    internal T? Value { get; }
    public T GetValueOrThrow()
    {
        if (IsSuccess)
            return Value!;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }
    public bool IsSuccess => Error == null;

    public static implicit operator Result(Result<T> result)
    {
        return new Result(result.Error);
    }
}
