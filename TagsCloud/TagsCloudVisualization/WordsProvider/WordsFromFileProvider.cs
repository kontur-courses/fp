using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultMonad;

namespace TagsCloudVisualization.WordsProvider
{
    public abstract class WordsFromFileProvider : IWordsProvider
    {
        protected readonly string PathToFile;

        protected WordsFromFileProvider(string pathToFile)
        {
            PathToFile = pathToFile;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            return PathToFile.AsResult()
                .Validate(File.Exists, $"File {PathToFile} not found")
                .ToNone()
                .Then(GetText)
                .Then(lines => lines.SelectMany(WordSplitter.Split));
        }

        protected abstract IEnumerable<string> GetText();
    }
}