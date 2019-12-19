using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Word;
using TagCloud.TextPreprocessor.Core;
using TagsCloud;

namespace TagCloud.TextPreprocessor.TextRiders
{
    public class MSWordTextRider : IFileTextRider
    {
        private TextRiderConfig textRiderConfig;
        
        public MSWordTextRider(TextRiderConfig textRiderConfig)
        {
            this.textRiderConfig = textRiderConfig;
        }

        public string[] ReadingFormats => new[] {".doc", ".docx"};
        public TextRiderConfig RiderConfig => textRiderConfig;

        public Result<IEnumerable<Tag>> GetTags()
        {
            return Result.Of(() => GetFileContent(textRiderConfig.FilePath)
                .Split(textRiderConfig.WordsDelimiters)
                .Select(str => textRiderConfig.GetCorrectWordFormat(str))
                .Where(str => !textRiderConfig.IsSkipWord(str))
                .Where(str => str != "")
                .Select(str => new Tag(str)));
        }
        
        private string GetFileContent(string filePath)
        {
            object refFullFilePath = filePath;
            var none = Type.Missing;
            var app = new Application();
            app.Documents.Open(ref refFullFilePath,
                ref none, ref none, ref none, ref none,
                ref none, ref none, ref none, ref none,
                ref none, ref none, ref none, ref none, ref none,
                ref none, ref none);

            var document = app.Documents.Application.ActiveDocument;
            object startIndex = 0;
            object endIndex = document.Characters.Count;
            var docRange = document.Range(ref startIndex, ref endIndex);
                
            var text = docRange.Text;
            app.Quit(ref none, ref none, ref none);
            return text;
        }
    }
}