using System;

namespace TagsCloud
{
    public static class ResultExtension
    {
        public static ResultHandler<T> Then<T>(
            this ResultHandler<T> input,
            Func<ResultHandler<T>, ResultHandler<T>> continuation)
        {
            return input.IsSuccess
                ? continuation(input)
                : input.Fail(input.Error);
        }

        public static ResultHandler<TOut> ThenDoWorkWithValue<T, TOut>(
            this ResultHandler<T> input,
            Func<T, TOut> continuation)
        {
            var newValue = continuation(input.Value);
            var handler = new ResultHandler<TOut>(newValue);

            return input.IsSuccess
                ? handler
                : handler.Fail(input.Error);
        }

        public static ResultHandler<T> ReplaceError<T>(
            this ResultHandler<T> input,
            Func<string, string> replaceError)
        {
            if (input.IsSuccess) return input;
            return input.Fail(replaceError(input.Error));
        }

        public static ResultHandler<T> RefineError<T>(
            this ResultHandler<T> input,
            string errorMessage)
        {
            return ReplaceError(input, err => errorMessage + ". " + err);
        }
    }
}