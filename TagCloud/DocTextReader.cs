using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using Microsoft.Office.Interop.Word;
using ResultOf;

namespace TagCloud
{
    public class DocTextReader : ITextReader
    {
        public Result<string> TryReadText(string fileName)
        {
            object filename = fileName;
            object missed = Type.Missing;
            var application = new Application();
            return Result.Of(() =>
            {
                application.Documents.Open(ref filename,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed,
                    ref missed);
                var document = application.Documents.Application.ActiveDocument;
                object start = 0;
                object stop = document.Characters.Count;
                var rng = document.Range(ref start, ref stop);
                var text = rng.Text;
                application.Quit(ref missed, ref missed, ref missed);
                return text;
            }, "File not found.");

        }
    }
}