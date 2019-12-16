using System;
using System.Collections.Generic;
using TagsCloud.Interfaces;
using System.Linq;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Spliters
{
    public class SpliterByLine : ITextSpliter
    {
        private string splitChar = Environment.NewLine;

        public Result<IEnumerable<string>> SplitText(string text)
        {
            return text == null ? Result.Fail<IEnumerable<string>>("Text is null") 
                : text.Split(splitChar).AsEnumerable().AsResult().Then(lines => lines.Where(line => !string.IsNullOrWhiteSpace(line)));
        }
    }
}
