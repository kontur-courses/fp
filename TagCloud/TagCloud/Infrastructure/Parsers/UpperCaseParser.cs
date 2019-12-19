using ResultOF;
using System.Linq;

namespace TagCloud
{
    public class UpperCaseParser : IParser
    {
        public bool IsChecked { get; set; }

        public UpperCaseParser()
        {
            IsChecked = true;
        }

        public Result<string[]> ParseWords(string[] words)
        {
            if (words == null)
                return Result.Fail<string[]>("Words cannot be null");
            return words
                .Select(word => word.ToUpper())
                .ToArray();
        }
    }
}
