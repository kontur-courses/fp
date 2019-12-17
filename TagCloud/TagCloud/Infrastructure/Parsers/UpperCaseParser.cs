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

        public Result<string[]> ParseWords(string[] words) => words
            .Select(word => word.ToUpper())
            .ToArray();
    }
}
