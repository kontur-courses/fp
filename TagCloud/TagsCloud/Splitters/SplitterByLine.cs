using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.Splitters
{
    public class SplitterByLine : ITextSplitter
    {
        private readonly string splitChar = Environment.NewLine;

        public Result<IEnumerable<string>> SplitText(string text)
        {
            return text?.Split(splitChar).AsEnumerable().AsResult()
                       .Then(lines => lines.Where(line => !string.IsNullOrWhiteSpace(line))) ?? Result.Fail<IEnumerable<string>>("Text is null");
        }
    }
}