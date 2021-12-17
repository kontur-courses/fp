using ResultOf;

namespace ResultExtensions;

public static class ResultExtensions
{
    public static Result<T> AsResult<T>(this T value)
    {
        return Result.Ok(value);
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(inp => Result.Of(() => continuation(inp)));
    }

    public static Result<TInput> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => Result.Of(() =>
        {
            continuation(inp);
            return inp;
        }));
    }

    public static Result<TInput> Then<TInput>(
        this Result<TInput> input,
        Action continuation)
    {
        return input.Then(inp => Result.Of(() =>
        {
            continuation();
            return inp;
        }));
    }

    public static Result Then(
        this Result input,
        Action continuation)
    {
        return input.Then(() => Result.OfAction(() => continuation()));
    }

    public static Result<TOutput> Then<TOutput>(
        this Result input,
        Func<TOutput> continuation)
    {
        return input.IsSuccess
            ? Result.Of(continuation)
            : Result.Fail<TOutput>(input.Error!);
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value!)
            : Result.Fail<TOutput>(input.Error!);
    }

    public static Result Then<TInput>(
        this Result<TInput> input,
        Func<TInput, Result> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value!)
            : Result.Fail(input.Error!);
    }

    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<string> handleError)
    {
        if (!input.IsSuccess)
            handleError(input.Error!);
        return input;
    }

    public static Result OnFail(
        this Result input,
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
        return Result.Fail<TInput>(replaceError(input.Error!));
    }

    public static Result ReplaceError(
        this Result input,
        Func<string, string> replaceError)
    {
        if (input.IsSuccess)
            return input;
        return Result.Fail(replaceError(input.Error!));
    }

    public static Result RefineError(
        this Result input,
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
