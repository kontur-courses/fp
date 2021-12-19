using System;
using System.IO;

namespace TagsCloudContainer.Common.Result
{
    public class ConsoleResultHandler : IResultHandler
    {
        private readonly TextReader reader;
        private readonly TextWriter writer;

        public ConsoleResultHandler(TextReader reader, TextWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }

        public void Handle(Action action, string error = null)
        {
            while (true)
            {
                writer.WriteLine();
                var result = Result.OfAction(action, error);
                writer.WriteLine();
                if (result.IsSuccess)
                    return;
                writer.WriteLine(result.Error + "\n");
            }
        }

        public void AddHandledText(string text)
        {
            writer.WriteLine(text);
        }

        public string GetText() 
            => reader.ReadLine();
    }
}