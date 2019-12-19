using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloud.FileParsers
{
    public class TxtParser : IFileParser
    {
        public string[] FileExtensions => new string[] { ".txt", ".md" };

        public Result<ImmutableList<string>> Parse(string filename)
        {
            return Result.Of(() => File.ReadAllLines(filename))
                .RefineError($"Can't read file '{filename}'.")
                .Then(lines => ImmutableList.ToImmutableList(lines.SelectMany(line => Regex.Split(line, @"\W+").Where(item => !string.IsNullOrEmpty(item)))));
        }
    }
}
