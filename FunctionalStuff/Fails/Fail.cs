using System;
using FunctionalStuff.Results;

namespace FunctionalStuff.Fails
{
    public static class Fail
    {
        public static FailureAssertionsProvider<T> If<T>(T value, string visibleName = null) =>
            new FailureAssertionsProvider<T>(true, value, visibleName);

        public static FailureAssertionsProvider<T> FailIf<T>(this T input, string visibleName = null)
        {
            return If(input, visibleName);
        }

        public static Result<TOut> ThenFailIf<T, TOut>(this Result<T> input,
            Func<FailureAssertionsProvider<T>, Result<TOut>> selector,
            string visibleName = null)
        {
            return input.Then(i => selector(i.FailIf(visibleName)));
        }
    }
}