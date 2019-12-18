using System;

namespace TagsCloudGenerator
{
    public class Result<T>
    {
        public string Error { get; }
        internal T Value { get; }

        public bool IsSuccess => string.IsNullOrEmpty(Error);

        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public T GetValueOrThrow()
        {
            if (IsSuccess)
                return Value;

            throw new InvalidOperationException("There is no value");
        }

        public static implicit operator Result<T>(T value)
        {
            return Result.Ok(value);
        }
    }

    public class None
    {
        private None()
        {
        }
    }


    public static class Result 
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null);
        }

        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(error);
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<T> OnFail<T>(
            this Result<T> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);

            return input;
        }

        public static Result<T> Of<T>(Func<T> factory, string error = null)
        {
            try
            {
                return Ok(factory());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<T> Then<T>(
            this Result<T> input,
            Action<T> converter)
        {
            return input.Then(t =>
            {
                converter(t);
                return t;
            });
        }


        public static Result<None> OfAction(Action f, string error = null)
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

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            return input.IsSuccess ? input : Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + ". " + err);
        }
    }
}