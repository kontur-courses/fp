using System;

namespace Functional
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly Maybe<T>.MaybeValueWrapper _value;

        public T Value
        {
            get
            {
                if (this.HasNoValue)
                    throw new InvalidOperationException();
                return this._value.Value;
            }
        }

        public static Maybe<T> None => new Maybe<T>();

        public bool HasValue => this._value != null;

        public bool HasNoValue => !HasValue;

        private Maybe(T value)
        {
            this._value = (object)value == null ? (Maybe<T>.MaybeValueWrapper)null : new Maybe<T>.MaybeValueWrapper(value);
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> From(T obj)
        {
            return new Maybe<T>(obj);
        }

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (maybe.HasNoValue)
                return false;
            return maybe.Value.Equals((object)value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj is T)
                obj = (object)new Maybe<T>((T)obj);
            if (!(obj is Maybe<T>))
                return false;
            return this.Equals((Maybe<T>)obj);
        }

        public bool Equals(Maybe<T> other)
        {
            if (this.HasNoValue && other.HasNoValue)
                return true;
            if (this.HasNoValue || other.HasNoValue)
                return false;
            return this._value.Value.Equals((object)other._value.Value);
        }

        public override int GetHashCode()
        {
            if (this.HasNoValue)
                return 0;
            return this._value.Value.GetHashCode();
        }

        public override string ToString()
        {
            if (this.HasNoValue)
                return "No value";
            return this.Value.ToString();
        }

        private class MaybeValueWrapper
        {
            internal readonly T Value;

            public MaybeValueWrapper(T value)
            {
                this.Value = value;
            }
        }
    }
}
