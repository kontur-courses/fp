using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer
{
    public class TextFileReader : ITextReader
    {
        private readonly string fileName;

        public TextFileReader(string fileName)
        {
            this.fileName = fileName;
        }

        public Result<IEnumerable<string>> GetLines()
        {
            if (!File.Exists(fileName))
                return Result.Fail<IEnumerable<string>>
                    (string.Format("Text file {0} is not found.\n" +
                                   "Check if file path is correct.", fileName));
            return Result.Ok(File.ReadLines(fileName));
        }
    }
}