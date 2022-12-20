using System;
using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class ResultHandler<T> : IResultHandler<T>
    {
        private readonly T value;

        public string Error { get; private set; }

        public T Value
        {
            get
            {
                if (IsSuccess) return value;
                throw new InvalidOperationException($"{Error}");
            }
        }

        public bool IsSuccess => Error == null;

        public ResultHandler(T value)
        {
            this.value = value;
        }

        public ResultHandler<T> Fail(string e)
        {
            Error = e;
            return this;
        }
    }
}