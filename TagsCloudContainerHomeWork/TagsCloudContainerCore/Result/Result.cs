using System;

namespace TagsCloudContainerCore.Result;

public readonly struct Result<T>
{
    public Result(string error, T value)
    {
        Error = error;
        Value = value;
    }

    public static implicit operator Result<T>(T v)
    {
        return ResultExtension.Ok(v);
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