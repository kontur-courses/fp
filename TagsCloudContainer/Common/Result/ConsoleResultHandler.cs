using System;
using System.IO;

namespace TagsCloudContainer.Common.Result
{
    public class ConsoleResultHandler : IResultHandler
    {
        private readonly TextWriter writer;

        public ConsoleResultHandler(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Handle(Action action)
        {
            while (true)
            {
                var result = Common.Result.Result.OfAction(action);
                if(result.IsSuccess)
                    return;
                writer.WriteLine(result.Error);
            }
        }
    }
}