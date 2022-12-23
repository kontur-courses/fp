using System.Collections.Generic;
using System.IO;
using System.Text;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.Infrastructure.Parsers
{
    public class TxtParser : IParser
    {
        private readonly TxtParserHelper helper;
        private readonly ParserSettings settings;

        public TxtParser(ParserSettings settings)
        {
            helper = new TxtParserHelper();
            this.settings = settings;
        }

        public string FileType => "txt";

        public Result<IEnumerable<string>> WordParse(string path)
        {
            var encoding = helper.Encodings[settings.Encoding];
            return Result.Ok(settings.TextType == TextType.OneWordOneLine
                ? OneWordOneLineParse(path, encoding)
                : LiteraryTextParse(path, encoding));
        }

        private static IEnumerable<string> OneWordOneLineParse(string path, Encoding encoding)
        {
            return File.ReadLines(path, encoding);
        }

        private IEnumerable<string> LiteraryTextParse(string path, Encoding encoding)
        {
            foreach (var word in helper.SelectAllWordsRegex
                         .Matches(File.ReadAllText(path, encoding)))
            {
                var res = word.ToString()?.Trim();
                yield return res;
            }
        }
    }
}