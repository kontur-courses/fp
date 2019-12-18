using System.IO;
using System;
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;

namespace TagsCloudContainer.Readers
{
    public class SimpleReader : IReader
    {
        private readonly string path;
        private readonly Dictionary<string, Func<string, string[]>> readers;

        public SimpleReader(string path)
        {
            this.path = path;

            readers = new Dictionary<string, Func<string, string[]>>
            {
                { "txt", pathToText => ReadOther(pathToText) },
                { "doc", pathToText => ReadDoc(pathToText) },
                { "docx", pathToText => ReadDoc(pathToText) }
            };
        }

        public Result<string[]> ReadAllLines()
        {
            var splitPath = path.Split('.');
            if (!File.Exists(path))
                return Result.Fail<string[]>("The specified file does not exist");
            if (!readers.ContainsKey(splitPath[splitPath.Length - 1]))
                return Result.Fail<string[]>("This file extension is not supported");
            return readers[splitPath[splitPath.Length - 1]](path);
        }

        private string[] ReadOther(string path)
        {
            string[] text;
            var stringSeparators = new[] { "\r\n" };
            using (var stream = new StreamReader(path))
            {
                text = stream.ReadToEnd().Split(stringSeparators, StringSplitOptions.None);
            }
            return text;
        }

        private string[] ReadDoc(string path)
        {
            var text = new List<string>();
            Word.Application app = new Word.Application();
            app.Documents.Open(path);
            Word.Document doc = app.ActiveDocument;
            for (int i = 1; i < doc.Paragraphs.Count; i++)
            {
                var word = doc.Paragraphs[i].Range.Text;
                text.Add(word.Substring(0, word.Length - 1));
            }
            doc.Close();
            app.Quit();
            return text.ToArray();
        }
    }
}
