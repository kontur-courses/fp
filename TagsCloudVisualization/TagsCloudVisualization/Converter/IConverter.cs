namespace TagsCloudVisualization.Converter
{
    public interface IConverter<T>
    {
        T Convert(T obj);
    }
}
