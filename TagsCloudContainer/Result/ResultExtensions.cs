namespace TagsCloudContainer.WordProcessing;

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
    
    public static Result<None> OfAction(Action f, string? error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? e.Message);
        }
    }
    
    public static Result<None> Ok()
    {
        return Ok<None>(null!);
    }

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input, Func<TInput, TOutput> continuation)
    {
        return input.Then(i => Of(() => continuation(i)));
    }
    
    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
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