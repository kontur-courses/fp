using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsReaders.FileReaders
{
    public interface IFileReader
    {
        Result<string> Read(string filename);
        bool CanRead(string extension);
    }
}