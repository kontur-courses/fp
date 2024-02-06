using TagsCloud.Formatters;

namespace TagsCloud.FileReaders;

public interface IFileReader
{
    string SupportedExtension { get; }
    IEnumerable<string> ReadContent(string filename, IPostFormatter postFormatter = null);
}