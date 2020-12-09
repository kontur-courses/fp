using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FunctionalStuff.Results.Fails
{
    public static class FailureAssertions
    {
        public static Result<string> NullOrEmpty(this IExtensibleFailureAssertionsProvider<string> provider)
        {
            return provider.Validate(string.IsNullOrEmpty, "empty");
        }

        public static Result<Dictionary<TKey, TValue>> NullOrEmpty<TKey, TValue>(
            this IExtensibleFailureAssertionsProvider<Dictionary<TKey, TValue>> provider)
        {
            return provider.Validate(x => x == null || x.Count == 0, "empty");
        }

        public static Result<T[]> NullOrEmpty<T>(this IExtensibleFailureAssertionsProvider<T[]> provider)
        {
            return provider.Validate(x => x == null || !x.Any(), "empty");
        }

        public static Result<T[]> NullOrEmpty<T>(this IExtensibleFailureAssertionsProvider<IEnumerable<T>> provider)
        {
            return provider.Value
                .ToArray()
                .FailIf(provider.Name)
                .NullOrEmpty();
        }

        public static Result<T> Null<T>(this IExtensibleFailureAssertionsProvider<T> provider)
            where T : class
        {
            return provider.Validate(x => x == null, "null");
        }

        public static Result<T?> Null<T>(this IExtensibleFailureAssertionsProvider<T?> provider)
            where T : struct
        {
            return provider.Validate(x => !x.HasValue, "null");
        }

        public static Result<CancellationToken> Canceled(
            this IExtensibleFailureAssertionsProvider<CancellationToken> provider)
        {
            return provider.ValidateWithMessage(
                t => t.IsCancellationRequested,
                provider.IsInverted
                    ? FailMessages.CancellationRequested
                    : "Cancellation not requested");
        }

        public static Result<T> TokenCanceled<T>(this IExtensibleFailureAssertionsProvider<T> provider,
            CancellationToken token)
        {
            return token.FailIf().Canceled().Then(_ => provider.Value);
        }

        public static Result<T> TokenCanceled<T>(this IExtensibleFailureAssertionsProvider<Result<T>> provider,
            CancellationToken token)
        {
            return token.FailIf().Canceled().Then(_ => provider.Value);
        }

        public static Result<T> Matches<T>(this IExtensibleFailureAssertionsProvider<T> provider,
            Predicate<T> check,
            string error)
        {
            return provider.ValidateWithMessage(check, error);
        }

        public static Result<T> Validate<T>(this IExtensibleFailureAssertionsProvider<T> provider,
            Predicate<T> predicate,
            string error)
        {
            var message = $"{provider.Name} is {(provider.IsInverted ? "" : "not ")}{error}";
            return provider.ValidateWithMessage(predicate, message);
        }
    }
}