using System.Collections.Generic;

namespace TagsCloudVisualization.WordProcessing.FileHandlers
{
    public interface IFileHandler
    {
        string PathToFile { get; }
        Result<IEnumerable<string>> ReadFile();
    }
}