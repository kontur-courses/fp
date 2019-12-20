using System.Collections.Generic;
using System.IO;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public class TxtReader : IWordProvider
    {
        private readonly string filePath;

        public TxtReader(string filePath)
        {
            this.filePath = filePath;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            return Result.Of<IEnumerable<string>>(() => File.ReadAllLines(filePath),
                $"Error reading file {filePath}");
        }
    }
}