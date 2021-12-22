using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Common.FileReaders
{
    public class TextFileReader : IFileReader
    {
        public Result<string> ReadFile(string path)
        {
            return Result.Of(() => File.ReadAllText(path))
                .RefineError("Невозможно прочитать файл:");
        }

        public Result<IEnumerable<string>> ReadLines(string path)
        {
            return Result.Of(() => File.ReadAllLines(path).AsEnumerable())
                .RefineError("Невозможно прочитать файл:");
        }
    }
}