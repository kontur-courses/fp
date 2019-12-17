using ResultOF;
using System.Linq;

namespace TagCloud
{
    public class LowerCaseParser : IParser
    {
        public bool IsChecked { get; set; }

        public string Name { get; }

        public LowerCaseParser()
        {
            IsChecked = true;
            Name = "LowerCase parser";
        }

        public Result<string[]> ParseWords(string[] words) => words
            .Select(word => word.ToLower())
            .ToArray();
    }
}
