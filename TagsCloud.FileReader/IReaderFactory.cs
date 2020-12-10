using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public interface IReaderFactory
    {
        Result<IWordsReader> GetReader(string extension);
    }
}