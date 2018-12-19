using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TagsCloud
{
    public class WordsFromMicrosoftWord : IWordCollection
    {
        private static readonly char[] Separators =
        {
            ',',
            '.',
            ' ',
            '(',
            ')',
            '!',
            '\t'
        };

        private readonly string path;

        public WordsFromMicrosoftWord(string path)
        {
            this.path = path;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            var words = new List<string>();
            if (File.Exists(path))
            {
                var document = WordprocessingDocument.Open(path, false);
                var body = document.MainDocumentPart.Document.Body;
                foreach (var paragraph in body.OfType<Paragraph>())
                foreach (var run in paragraph.OfType<Run>())
                foreach (var text in run.OfType<Text>())
                foreach (var word in text.InnerText.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
                    words.Add(word);

                return words;
            }

            return Result.Fail<IEnumerable<string>>("File doesn`t exist");
        }
    }
}