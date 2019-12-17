using ResultOF;
using System.Linq;

namespace TagCloud
{
    public class UpperCaseParser : IParser
    {
        public bool IsChecked { get; set; }

        public string Name { get; }

        public UpperCaseParser()
        {
            IsChecked = true;
            Name = "UpperCase parser";
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
