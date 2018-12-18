using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagCloud.Data;
using TagCloud.Reader.FormatReader;

namespace TagCloud.Reader
{
    public class TextFileReader : IWordsFileReader
    {
        private readonly Dictionary<string, IFormatReader> formatReaders;

        public TextFileReader(IEnumerable<IFormatReader> readers)
        {
            formatReaders = readers.ToDictionary(reader => reader.Format);
        }

        public static string GetFormat(string fileName) => Regex.Match(fileName, ".+\\.(.+)$").Groups[1].Value;

        public Result<IEnumerable<string>> ReadWords(string fileName)
        {
            return Result.Of(() => ReadText(fileName, GetFormat(fileName)), $"Can't read file {fileName}")
                .Then(text => new Regex("\\p{L}+")
                    .Matches(text)
                    .Cast<Match>()
                    .Select(match => match.Value));
        }

        private string ReadText(string fileName, string format)
        {
            return formatReaders.TryGetValue(format, out var reader) 
                ? reader.Read(fileName) 
                : File.ReadAllText(fileName, Encoding.Default);
        }
    }
}