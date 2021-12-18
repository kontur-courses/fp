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
                var result = Result.OfAction(action, error);
                if(result.IsSuccess)
                    return;
                writer.WriteLine(result.Error);
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