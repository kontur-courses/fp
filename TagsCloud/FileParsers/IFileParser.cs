using System.Collections.Immutable;

namespace TagsCloud.FileParsers
{
    public interface IFileParser
    {
        string[] FileExtensions { get; }
        Result<ImmutableList<string>> Parse(string filename);
    }
}
