namespace TagsCloud.Interfaces
{
    public interface IRecognizer<T>
    {
        public Result<T> Recognize(T value);
    }
}