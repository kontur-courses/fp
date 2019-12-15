using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudContainer.FileReading
{
    public class TextFileReader : IFileReader
    {
        public IEnumerable<string> ReadWords(string textFileName)
        {
            if (!File.Exists(textFileName))
                throw new FileNotFoundException($"File {textFileName} was not found");

            var notAllowedSymbolsRegex = new Regex(@"[^\w+ ]");

            return File
                .ReadLines(textFileName)
                .SelectMany(line => notAllowedSymbolsRegex
                    .Replace(line, " ")
                    .Split(' ')
                    .Where(w => w.Length > 0)
                );
        }
    }
}