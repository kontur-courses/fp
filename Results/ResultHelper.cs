using ResultOf;

namespace ResultHelper;

public static class Result
{
    public static Result<T> AsResult<T>(this T value)
    {
        return Ok(value);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }
    public static ResultOf.Result Ok()
    {
        return new(null);
    }

    public static ResultOf.Result Fail(string e)
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
            return Fail<T>(error ?? e.Message);
        }
    }

    public static ResultOf.Result OfAction(Action f, string? error = null)
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

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(inp => Of(() => continuation(inp)));
    }

    public static ResultOf.Result Then<TInput, TOutput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static ResultOf.Result Then(
        this ResultOf.Result input,
        Action continuation)
    {
        return input.Then(() => OfAction(() => continuation()));
    }

    public static ResultOf.Result Then(
        this ResultOf.Result input,
        Func<ResultOf.Result> continuation)
    {
        return input.IsSuccess
            ? continuation()
            : Fail(input.Error!);
    }

    public static Result<TOutput> Then<TOutput>(
        this ResultOf.Result input,
        Func<Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation()
            : Fail<TOutput>(input.Error!);
    }

    public static ResultOf.Result Then<TInput>(
        this Result<TInput> input,
        Func<TInput, ResultOf.Result> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value!)
            : Fail(input.Error!);
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value!)
            : Fail<TOutput>(input.Error!);
    }

    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<string> handleError)
    {
        if (!input.IsSuccess)
            handleError(input.Error!);
        return input;
    }

    public static ResultOf.Result OnFail(
        this ResultOf.Result input,
        Action<string> handleError)
    {
        if (!input.IsSuccess)
            handleError(input.Error!);
        return input;
    }

    public static Result<TInput> ReplaceError<TInput>(
        this Result<TInput> input,
        Func<string, string> replaceError)
    {
        if (input.IsSuccess)
            return input;
        return Fail<TInput>(replaceError(input.Error!));
    }

    public static ResultOf.Result ReplaceError(
        this ResultOf.Result input,
        Func<string, string> replaceError)
    {
        if (input.IsSuccess)
            return input;
        return Fail(replaceError(input.Error!));
    }

    public static ResultOf.Result RefineError(
        this ResultOf.Result input,
        string errorMessage)
    {
        return input.ReplaceError(err => errorMessage + ". " + err);
    }

    public static Result<TInput> RefineError<TInput>(
        this Result<TInput> input,
        string errorMessage)
    {
        return input.ReplaceError(err => errorMessage + ". " + err);
    }
}
