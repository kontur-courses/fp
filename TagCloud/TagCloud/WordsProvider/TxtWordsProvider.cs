using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class TxtWordsProvider : FileWordsProvider
    {
        public TxtWordsProvider(string filePath) : base(filePath)
        {
            SupportedExtensions = new[] {"txt"};
            if (!CheckFile(filePath))
                throw new ArgumentException($"Incorrect file {filePath}");
        }

        public override string[] SupportedExtensions { get; }

        public override Result<IEnumerable<string>> GetWords()
        {
            var words = Result.Of<IEnumerable<string>>(() =>
                Regex.Split(File.ReadAllText(FilePath), @"\W+"));
            return words;
        }
    }
}