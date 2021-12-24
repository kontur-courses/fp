namespace TagCloud.selectors
{
    public interface IConverter<T>
    {
        Result<T> Convert(T source);
    }
}