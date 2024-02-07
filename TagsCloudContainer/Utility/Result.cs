namespace TagsCloudContainer.Utility
{  
    public readonly struct Result<T>
    {
        public T Value { get; }
        public string Error { get; }

        public Result(string error, T value = default!)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return Result.Ok(value);
        }

        public bool IsSuccess => Error is null;

        public T GetValueOrThrow()
        {
            if (IsSuccess)
                return Value;
            throw new InvalidOperationException($"Failed operation with error: {Error}");
        }

        public void OnSuccess(Action<T> onSuccess)
        {
            if (IsSuccess)
            {
                onSuccess(Value);
            }
        }

        public void OnFail(Action<string> onFail)
        {
            if (!IsSuccess)
            {
                onFail(Error);
            }
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
        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null!, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null!);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(message);
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

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }

        public static void OnSuccess<T>(this Result<T> result, Action<T> onSuccess)
        {
            result.OnSuccess(onSuccess);
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            return input.IsSuccess
                ? input
                : Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + ". " + err);
        }


    }


}
