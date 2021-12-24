using System;
using System.Diagnostics.CodeAnalysis;

namespace TagsCloudContainerCore.Result;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ResultExtension
{
    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null!, value);
    }

    public static Result<None> Ok()
    {
        return Ok<None>(null!);
    }

    public static Result<T> Fail<T>(string e)
    {
        return new Result<T>(e, default(T)!);
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(inp => Of(() => continuation(inp)));
    }

    // ReSharper disable once UnusedTypeParameter
    public static Result<None> Then<TInput, TOutput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    // ReSharper disable once MemberCanBePrivate.Global

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value)
            : Fail<TOutput>(input.Error);
    }

    public static Result<TInput> OnSuccess<TInput>(
        this Result<TInput> input,
        Action<string> handleSuccess,
        string information = ""
    )
    {
        if (input.IsSuccess) handleSuccess(information);
        return input;
    }

    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<string> handleError)
    {
        if (!input.IsSuccess) handleError(input.Error);
        return input;
    }

    private static Result<T> Of<T>(Func<T> f, string? error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? $"{e.GetType().Name}: {e.Message}");
        }
    }

    private static Result<None> OfAction(Action f, string? error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? $"{e.GetType().Name} : {e.Message}");
        }
    }
}