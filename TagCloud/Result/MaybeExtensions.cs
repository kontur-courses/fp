using System;

namespace Functional
{
    public static class MaybeExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) =>
            maybe.HasNoValue ? Result.Fail<T>(errorMessage) : maybe.Value.AsResult();

        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = default(T))
        {
            return maybe.Unwrap(x => x, defaultValue);
        }

        public static K Unwrap<T, K>(this Maybe<T> maybe, Func<T, K> selector, K defaultValue = default(K))
        {
            if (maybe.HasValue)
                return selector(maybe.Value);
            return defaultValue;
        }

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            if (maybe.HasNoValue || !predicate(maybe.Value))
                return Maybe<T>.None;
            return maybe;
        }

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, K> selector) =>
            maybe.HasNoValue ? Maybe<K>.None : selector(maybe.Value);

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, Maybe<K>> selector) =>
            maybe.HasNoValue ? Maybe<K>.None : selector(maybe.Value);

        public static void Execute<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe.HasNoValue)
                return;
            action(maybe.Value);
        }
    }
}
