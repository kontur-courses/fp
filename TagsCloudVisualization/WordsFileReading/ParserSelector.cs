using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public class ParserSelector
    {
        private readonly IDictionary<string, IParser> parserByMode;

        public ParserSelector(IEnumerable<IParser> parsers)
        {
            parserByMode = new Dictionary<string, IParser>();

            foreach (var parser in parsers)
                parserByMode[parser.GetModeName()] = parser;
        }

        public Result<IParser> SelectParser(string mode)
        {
            if (parserByMode.ContainsKey(mode))
                return Result.Ok(parserByMode[mode]);
            return Result.Fail<IParser>(
                $"Parsing mode is not supported. Supported modes are: {string.Join(", ", parserByMode.Keys)}");

        }
    }
}
