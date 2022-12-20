namespace TagsCloud.Interfaces
{
    public interface IResultHandler<T>
    {
        public string Error { get; }

        public T Value { get; }

        public bool IsSuccess => Error == null;
    }
}