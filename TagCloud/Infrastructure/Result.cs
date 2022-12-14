namespace TagCloud.Infrastructure;

public class Result<T>
{
    public string? Error { get; }
    public T? Value { get; }
    public bool IsSuccess => Error == null;

    public Result(string? error, T? value = default)
    {
        Error = error;
        Value = value;
    }
    
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static implicit operator T(Result<T> result)
    {
        return result.Value;
    } 
}

public static class Result
{
    public static Result<TIn> OnFail<TIn>(this Result<TIn> input, Action<string> handleError)
    {
        if (!input.IsSuccess) 
            handleError(input.Error);

        return input;
    }
    
    public static Result<TIn> OnSuccess<TIn>(this Result<TIn> input, Action<TIn> handleSuccess)
    {
        if (input.IsSuccess) 
            handleSuccess(input);
        
        return input;
    }
}