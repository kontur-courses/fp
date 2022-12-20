namespace TagsCloud.Interfaces
{
    public interface IRecognizer<T>
    {
        public ResultHandler<T> Recognize(T value);
    }
}