using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TagsCloud.FileParsers
{
    public class TxtParser : IFileParser
    {
        public string[] FileExtensions => new string[] { ".txt", ".md" };

        public Result<ImmutableList<string>> Parse(string filename)
        {
            //todo without separators
            var separators = new char[] { ' ', ',', '.', '!', '?', '(', ')', '{', '}', '[', ']' };
            return Result.Of(() => File.ReadAllLines(filename))
                .RefineError($"Can't read file '{filename}'.")
                .Then(lines => ImmutableList.ToImmutableList(lines.SelectMany(line => line.Split(separators, System.StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
