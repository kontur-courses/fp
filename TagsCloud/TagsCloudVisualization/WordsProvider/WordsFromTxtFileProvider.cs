using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.WordsProvider
{
    public class WordsFromTxtFileProvider : WordsFromFileProvider
    {
        public WordsFromTxtFileProvider(string pathToFile) : base(pathToFile)
        {
        }

        protected override IEnumerable<string> GetText() => File.ReadLines(PathToFile);
    }
}