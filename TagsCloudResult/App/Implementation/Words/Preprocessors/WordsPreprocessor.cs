using System.Collections.Generic;
using System.Linq;

namespace App.Implementation.Words.Preprocessors
{
    public class WordsPreprocessor
    {
        public IEnumerable<string> LeadingWordsToLowerCase(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower());
        }
    }
}