using System.Collections.Immutable;
using System.IO;
using System.Linq;
using TagsCloud.FileParsers;

namespace TagsCloud
{
    public class WordsLoader
    {
        private readonly IFileParser[] parsers;

        public WordsLoader(IFileParser[] parsers)
        {
            this.parsers = parsers;
        }

        public Result<ImmutableList<string>> LoadWords(string filename)
        {
            return Result.Of(() => Path.GetExtension(filename))
                .Then(fileExtension => parsers.First(p => p.FileExtensions.Any(ext => ext == fileExtension)))
                .ReplaceError(msg => $"Can't select file parser for this file format ({filename})")
                .Then(fileParser => fileParser.Parse(filename))
                .RefineError($"Can't load words from file '{filename}'");
        }
    }
}
