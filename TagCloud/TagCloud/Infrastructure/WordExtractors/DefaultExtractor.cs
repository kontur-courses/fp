using ResultOF;
using System;
using System.Linq;

namespace TagCloud
{
    public class DefaultExtractor : IExtractor
    {
        public Result<string[]> ExtractWords(string textResult)
        {
            var text = textResult;
            if (text == null) return Result.Fail<string[]>("No text!");
            var words = text.Split('\r', '\n');
            return words.Where(word => word != "").ToArray();
        }
    }
}
