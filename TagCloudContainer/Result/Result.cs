using System;
using System.Drawing;

namespace TagCloudContainer.TaskResult
{
    public struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return new Result<T>(null, v);
        }

        public string Error { get; }
        public T Value { get; }

        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> OnSuccess<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<T> OnFail<T>(string error)
        {
            return new Result<T>(error);
        }


        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : OnFail<TOutput>(input.Error);
        }

        public static void Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            if (input.IsSuccess)
                continuation(input.Value);
            else
                Console.WriteLine(input.Error);
        }
    }
}