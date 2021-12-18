using System;

namespace ResultMonad
{
    public static class ResultNoneExtensions
    {
        public static Result<None> ToNone<TInput>(this Result<TInput> input) => input.Then(_ => Result.Ok());

        public static Result<TOutput> Then<TOutput>(this Result<None> input, Func<Result<TOutput>> continuation) =>
            input.Then(_ => continuation());

        public static Result<TOutput> Then<TOutput>(this Result<None> input, Func<TOutput> continuation) =>
            input.Then(_ => continuation());

        public static Result<None> Validate(this Result<None> input, Func<bool> validate, string err) =>
            input.Validate(_ => validate(), err);

        public static Result<None> ValidateNonNull<T>(this Result<None> input, T param, string paramName) =>
            input.Validate(() => param is not null, $"{paramName} was null");

        public static Result<TOutput> ToValue<TOutput>(this Result<None> input, TOutput output) =>
            input.Then(() => output);
    }
}