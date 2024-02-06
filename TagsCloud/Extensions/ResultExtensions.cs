using TagsCloud.Results;

namespace TagsCloud.Extensions;

public static class ResultExtensions
{
    public static Result<T> AsResult<T>(this T value)
    {
        return Ok(value);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static Result<None> Ok()
    {
        return Ok<None>(null);
    }

    public static Result<T> Fail<T>(string error)
    {
        return new Result<T>(error);
    }

    public static Result<T> Of<T>(Func<T> func, string error = null)
    {
        try
        {
            return Ok(func());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? e.Message);
        }
    }

    public static Result<None> OfAction(Action action, string error = null)
    {
        try
        {
            action();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? e.Message);
        }
    }

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(inp => Of(() => continuation(inp)));
    }

    public static Result<None> Then<TInput>(this Result<TInput> input, Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess ? continuation(input.Value) : Fail<TOutput>(input.Error);
    }

    public static Result<TInput> OnFail<TInput>(this Result<TInput> input, Action<string> handleError)
    {
        if (!input.IsSuccess)
            handleError(input.Error);

        return input;
    }

    public static Result<TInput> ReplaceError<TInput>(this Result<TInput> input,
        Func<string, string> replaceError)
    {
        return input.IsSuccess ? input : Fail<TInput>(replaceError(input.Error));
    }

    public static Result<TInput> RefineError<TInput>(this Result<TInput> input, string error)
    {
        return input.ReplaceError(err => error + " " + err);
    }
}