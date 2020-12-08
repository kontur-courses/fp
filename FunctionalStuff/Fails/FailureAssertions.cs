using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunctionalStuff.Results;

namespace FunctionalStuff.Fails
{
    public static class FailureAssertions
    {
        public static Result<string> NullOrEmpty(
            this FailureAssertionsProvider<string> provider)
        {
            return provider.Validate(string.IsNullOrEmpty, "empty");
        }

        public static Result<Dictionary<TKey, TValue>> NullOrEmpty<TKey, TValue>(
            this FailureAssertionsProvider<Dictionary<TKey, TValue>> provider)
        {
            return provider.Validate(x => x == null || x.Count == 0, "empty");
        }

        public static Result<T[]> NullOrEmpty<T>(
            this FailureAssertionsProvider<T[]> provider)
        {
            return provider.Validate(x => x == null || !x.Any(), "empty");
        }

        public static Result<T[]> NullOrEmpty<T>(this FailureAssertionsProvider<IEnumerable<T>> provider)
        {
            return provider.Value
                .ToArray()
                .FailIf(provider.Name)
                .NullOrEmpty();
        }

        public static Result<T> Null<T>(this FailureAssertionsProvider<T> provider)
            where T : class
        {
            return provider.Validate(x => x == null, "null");
        }

        public static Result<T?> Null<T>(this FailureAssertionsProvider<T?> provider)
            where T : struct
        {
            return provider.Validate(x => !x.HasValue, "null");
        }

        public static Result<CancellationToken> Cancelled(this FailureAssertionsProvider<CancellationToken> provider)
        {
            return provider.ValidateWithMessage(
                t => t.IsCancellationRequested,
                provider.IsInverted
                    ? General.Tasks.CancellationRequested
                    : "Cancellation not requested");
        }

        public static Result<T> TokenCancelled<T>(this FailureAssertionsProvider<T> provider, CancellationToken token)
        {
            return token.FailIf().Cancelled().Then(_ => provider.Value);
        }

        public static Result<T> TokenCancelled<T>(this FailureAssertionsProvider<Result<T>> provider,
            CancellationToken token)
        {
            return token.FailIf().Cancelled().Then(_ => provider.Value);
        }

        public static Result<T> Matches<T>(this FailureAssertionsProvider<T> provider, Predicate<T> check, string error)
        {
            return provider.Validate(check, error);
        }
    }
}