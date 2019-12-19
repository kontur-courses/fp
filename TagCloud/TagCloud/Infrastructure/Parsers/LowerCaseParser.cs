using ResultOF;
using System.Linq;

namespace TagCloud
{
    public class LowerCaseParser : IParser
    {
        public bool IsChecked { get; set; }

        public LowerCaseParser()
        {
            IsChecked = true;
        }

        public Result<string[]> ParseWords(string[] words)
        {
            if (words == null)
                return Result.Fail<string[]>("Words cannot be null");
            return words
                .Select(word => word.ToLower())
                .ToArray();
        }
    }
}
