using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.Settings;
using Application = Microsoft.Office.Interop.Word.Application;

namespace TagsCloudVisualization
{
    public class WordsExtractor : IWordsExtractor
    {

        private readonly IWordsExtractorSettings settings;

        public WordsExtractor(IWordsExtractorSettings settings)
        {
            this.settings = settings;
        }

        public Result<List<string>> Extract(string path)
        {
            return ValidateFormatIsSupported(path)
                .Then(t => GetTotalText(path))
                .Then(ReplaceSpecialCharacters)
                .Then(DeleteStopWords)
                .OnFail(Console.WriteLine);
        }

        private Result<List<string>> DeleteStopWords(string text)
        {
            return text.Split(' ')
                .Where(w => w.Length >= 3 && w != string.Empty && !settings.StopWords.Contains(w))
                .Select(w => w.Trim().ToLowerInvariant()).ToList();
        }

        private Result<string> ReplaceSpecialCharacters(string text)
        {
            var textWithoutStopChars = settings.StopChars.Aggregate(text, (current, c) => current.Replace(c, ' '));
            return textWithoutStopChars.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }

        private Result<string> GetTotalText(string path)
        {
            var format = Path.GetExtension(path);
            if (format != null && format.Equals(".doc"))
            {
                var textBuilder = new StringBuilder();
                var word = new Application();
                object miss = Missing.Value;
                object fileName = Path.IsPathRooted(path)
                    ? path
                    : $"{System.Windows.Forms.Application.StartupPath}\\{path}";
                var docs = word.Documents.Open(ref fileName, ref miss, ref miss, ref miss, ref miss, ref miss,
                    ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);

                for (var i = 0; i < docs.Paragraphs.Count; i++)
                    textBuilder.Append(docs.Paragraphs[i + 1].Range.Text);

                docs.Close();
                word.Quit();
                return textBuilder.ToString();
            }

            if (format != null && format.Equals(".txt"))
                return File.ReadAllText(path, Encoding.Default);

            return null;
        }

        private Result<string> ValidateFormatIsSupported(string path)
        {
            var format = Path.GetExtension(path);
            return Result.Validate(format, f => f == ".txt" || f == ".doc" || f == ".docx",
                $"Invalid format '{format}'");
        }
    }
}