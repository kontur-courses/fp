using System.Collections.Generic;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Common.FileReaders
{
    public interface IFileReader
    {
        public Result<string> ReadFile(string path);
        
        public Result<IEnumerable<string>> ReadLines(string path);
    }
}