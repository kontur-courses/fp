using System;

namespace TagsCloud.Infrastructure
{    
    public readonly struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
        }
        public string Error { get; }
        internal T Value { get; }
        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}... Пошёл на хуй");
        }
        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value) => Ok(value);

        public static Result<T> Ok<T>(T value) => new Result<T>(null, value);

        public static Result<T> Fail<T>(string e) => new Result<T>(e);

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

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation) =>
            Then(input, inp => Of(() => continuation(inp)));

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation) =>
            input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error); //очередной фейл в моей жизни


        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> error) =>
         input.IsSuccess
                ? input
                : new Result<TInput>(error(input.Error), input.Value);

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string error) =>
        input.ReplaceError(e => $"{error}. {e}");
    }
}
