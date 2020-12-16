using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class MicrosoftWordWordsProvider : FileWordsProvider
    {
        public MicrosoftWordWordsProvider(string filePath) : base(filePath)
        {
            SupportedExtensions = new[] {"doc", "docx"};
            if (!CheckFile(filePath))
                throw new ArgumentException($"Incorrect file {filePath}");
        }

        public override string[] SupportedExtensions { get; }

        public override Result<IEnumerable<string>> GetWords()
        {
            var application = new Application();
            var document = application.Documents.Open(FilePath);
            var words = (from Range word in document.Words
                    where Regex.IsMatch(word.Text, @"\w+")
                    select word.Text)
                .ToList();
            application.Quit();
            return words;
        }
    }
}