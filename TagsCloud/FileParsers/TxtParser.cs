using System;
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
            var separators = new char[] { ' ', ',', '.', '!', '?', '(', ')', '{', '}', '[', ']' };

            return Result.Of(() => File.ReadAllLines(filename))
                .RefineError($"Can't read file '{filename}'.")
                .Then(lines => ImmutableList<string>.Empty.AddRange(lines.SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries))));
        }
    }
}
