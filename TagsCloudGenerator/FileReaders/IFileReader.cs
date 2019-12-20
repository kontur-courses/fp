using System.Collections.Generic;
using FunctionalTools;

namespace TagsCloudGenerator.FileReaders
{
    public interface IFileReader
    {
        string TargetExtension { get; }
        Result<Dictionary<string, int>> ReadWords(string path);
    }
}