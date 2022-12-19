using System;

namespace TagsCloud
{
    public static class ResultExtension
    {
        public static Result<TOutput> Then<T, TOutput>(
            this Result<T> input,
            Func<T, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Result<T>.Fail<TOutput>(input.Error);
        }

        public static Result<TOutput> ThenDoWorkWithValue<T, TOutput>(
           this Result<T> input,
           Func<T, TOutput> continuation)
        {
            var newValue = continuation(input.Value);

            return input.IsSuccess
                ? Result<TOutput>.Ok(newValue)
                : Result<T>.Fail<TOutput>(input.Error);
        }

        public static Result<T> ReplaceError<T>(
           this Result<T> input,
           Func<string, string> replaceError)
        {
            if (input.IsSuccess) return input;
            return Result<T>.Fail<T>(replaceError(input.Error));
        }

        public static Result<T> RefineError<T>(
            this Result<T> input,
            string errorMessage)
        {
            return ReplaceError(input, err => errorMessage + ". " + err);
        }
    }
}
