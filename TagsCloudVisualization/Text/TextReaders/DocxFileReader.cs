using System;
using System.Collections.Generic;
using TagsCloudVisualization.Utils;
using MsWord = Microsoft.Office.Interop.Word;

namespace TagsCloudVisualization.Text.TextReaders
{
    public class DocxFileReader : ITextReader
    {
        private readonly char[] separators = {' ', '\n', '\t'};
        private MsWord.Application application;

        public DocxFileReader()
        {
        }

        public DocxFileReader(char[] separators)
        {
            this.separators = separators;
        }

        public HashSet<string> Formats { get; } = new HashSet<string> {"docx", "doc"};

        public Result<IEnumerable<string>> GetAllWords(string filepath)
        {
            return filepath.AsResult().Then(GetAllWordsAsEnumerable);
        }

        private IEnumerable<string> GetAllWordsAsEnumerable(string filepath)
        {
            application = new MsWord.Application { Visible = false };

            var document = application.Documents.Open(filepath);

            foreach (var word in document.Range().Text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                yield return word;

            document.Close();
            application.Quit();
        }
    }
}