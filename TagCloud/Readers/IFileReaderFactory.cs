using TagCloud.ResultMonad;

namespace TagCloud.Readers
{
    public interface IFileReaderFactory
    {
        Result<IFileReader> Create(string filename);
    }
}
