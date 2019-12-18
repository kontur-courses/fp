using Microsoft.Office.Interop.Word;
using SyntaxTextParser.Architecture;
using System;
using System.IO;
using ResultPattern;

namespace SyntaxTextParser
{
    public class MsWordFileReader : IFileReader
    {
        public Result<string> ReadTextFromFile(string filePath)
        {
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            return Result.Of(() => ReadDocxText(fullFilePath));
        }

        private string ReadDocxText(object fullFilePath)
        {
            var none = Type.Missing;
            var app = new Application();

            #region application.Documents.Open(refFilePath)
            app.Documents.Open(ref fullFilePath,
                ref none, ref none, ref none, ref none,
                ref none, ref none, ref none, ref none,
                ref none, ref none, ref none, ref none, ref none,
                ref none, ref none);
            #endregion

            var document = app.Documents.Application.ActiveDocument;
            object startIndex = 0;
            object endIndex = document.Characters.Count;
            var docRange = document.Range(ref startIndex, ref endIndex);

            var text = docRange.Text;
            app.Quit(ref none, ref none, ref none);

            return text;
        }

        public bool CanReadThatType(string type)
        {
            return type == "doc" || type == "docx";
        }
    }
}