using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyStemWrapper;

namespace TagsCloudContainer.Processing.Converting
{
    public class InitialFormConverter : IWordConverter
    {
        private static readonly MyStem Analyzer;
        private static readonly Regex WordRegex;


        static InitialFormConverter()
        {
            Analyzer = new MyStem
            {
                Parameters = "-in",
                PathToMyStem = @"Resources\mystem.exe"
            };

            WordRegex = new Regex(@"^([\w-]+?){([\w-]+?)=", RegexOptions.Multiline | RegexOptions.Compiled);
        }

        public IEnumerable<string> Convert(IEnumerable<string> words)
        {
            var validWords = words.Distinct().Where(w => !string.IsNullOrEmpty(w) && !w.Contains(" "));
            var analysis = Analyzer.Analysis(string.Join(" ", validWords));

            var matches = WordRegex.Matches(analysis);
            foreach (Match match in matches)
                yield return match.Groups[2].Value;
        }
    }
}