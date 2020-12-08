using System;
using FunctionalStuff.Results;

namespace FunctionalStuff.Fails
{
    public class FailureAssertionsProvider<T>
    {
        internal string Name { get; }
        internal bool IsInverted { get; }
        internal T Value { get; }

        public FailureAssertionsProvider(bool isInverted, T value, string name = null)
        {
            IsInverted = isInverted;
            Value = value;
            this.Name = name ?? typeof(T).Name;
        }

        public FailureAssertionsProvider<T> Not => new FailureAssertionsProvider<T>(!IsInverted, Value, Name);

        internal Result<T> Validate(Predicate<T> predicate, string error)
        {
            return ValidateWithMessage(predicate, $"{Name} is {(IsInverted ? "" : "not ")}{error}");
        }

        internal Result<T> ValidateWithMessage(Predicate<T> predicate, string message)
        {
            var isMatched = predicate.Invoke(Value);
            var isValid = IsInverted ? !isMatched : isMatched;
            return isValid ? Result.Ok(Value) : Result.Fail<T>(message);
        }
    }
}