using System.Collections.Generic;

namespace TagsCloudGenerator.FileReaders
{
    public interface IFileReader
    {
        string TargetExtension { get; }
        Result<Dictionary<string, int>> ReadWords(string path);
    }
}