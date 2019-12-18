using System.Collections.Generic;
using System.IO;
using TagsCloudGenerator.Tools;

namespace TagsCloudGenerator.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public string TargetExtension => "txt";

        private readonly IWordsParser parser;

        public TxtFileReader(IWordsParser parser)
        {
            this.parser = parser;
        }

        public Result<Dictionary<string, int>> ReadWords(string path)
        {
            return Result
                .Of(() => File.ReadAllText(path))
                .Then(text => parser.Parse(text))
                .Then(ParseHelper.GetWordToCount)
                .RefineError($"Count not read word from file {path}");
        }
    }
}