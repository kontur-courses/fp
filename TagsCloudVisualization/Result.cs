using System;

namespace TagsCloudVisualization
{
    public class None
    {
        private None()
        {
        }
    }
    
    public struct Result<T>
    {
        public readonly string Error;
        public readonly T Value;
        public bool IsSuccess => Error == null;
        
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<T> Ok<T>(T value = default(T))
        {
            return new Result<T>(null, value);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
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
        
        public static Result<None> Of(Action f, string error = null)
        {
            try
            {
                f();
                return Ok<None>();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e.Message);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.IsSuccess ? Of(() => continuation(input.Value)) : Fail<TOutput>(input.Error);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess ? continuation(input.Value) : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }
    }
}