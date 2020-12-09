using System;

namespace FunctionalStuff.Results.Fails
{
    public class FailureAssertionsProvider<T> : IExtensibleFailureAssertionsProvider<T>
    {
        private readonly T value;
        private readonly bool isInverted;
        private readonly string name;

        public FailureAssertionsProvider(bool isInverted, T value, string name = null)
        {
            this.isInverted = isInverted;
            this.value = value;
            this.name = name ?? typeof(T).Name;
        }

        T IExtensibleFailureAssertionsProvider<T>.Value => value;
        bool IExtensibleFailureAssertionsProvider<T>.IsInverted => isInverted;
        string IExtensibleFailureAssertionsProvider<T>.Name => name;

        public FailureAssertionsProvider<T> Not => new FailureAssertionsProvider<T>(!isInverted, value, name);

        Result<T> IExtensibleFailureAssertionsProvider<T>.ValidateWithMessage(Predicate<T> predicate, string message)
        {
            var isMatched = predicate.Invoke(value);
            var isValid = isInverted ? !isMatched : isMatched;
            return isValid ? Result.Ok(value) : Result.Fail<T>(message);
        }
    }
}