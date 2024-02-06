namespace TagsCloudContainer.WordProcessing;

public class None
{
    private None()
    {
    }
}
public class Result <T>
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

public static class Result
{
    public static Result<T> Ok<T>(this T value)
    {
        return new Result<T>(null, value);
    }

    public static Result<T> Fail<T>(string exception)
    {
        return new Result<T>(exception);
    }

    public static Result<T> Of<T>(Func<T> f, string error = null)
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

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> continuation)
    {
        return input.Then(i => Of(() => continuation(i)));
    }
    
    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value)
            : Fail<TOutput>(input.Error);
    }
    
    public static Result<T> OnFail<T>(this Result<T> input, Action<string> continuation)
    {
        if (!input.IsSuccess)
            continuation(input.Error);
        return input;
    }
    
    public static Result<T> ReplaceError<T>(this Result<T> input, Func<string, string> error)
    {
        return input.IsSuccess
            ? input
            : Fail<T>(error(input.Error));
    }
    
    public static Result<T> RefineError<T>(this Result<T> input, string exception)
    {
        return input.ReplaceError(e => $"{exception}. {input.Error}");
    }
}