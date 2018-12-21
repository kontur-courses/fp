using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;

namespace TagCloud
{
    public class FileWordSource : IWordSource
    {
        private readonly string filename;

        public FileWordSource(string filename)
        {
            this.filename = filename;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            return Result.Of(() => new StreamReader(filename))
                .RefineError($"Error when trying to read file {filename}")
                .Then(s => s.ReadToEnd().Split().AsEnumerable());
        }
    }
}
