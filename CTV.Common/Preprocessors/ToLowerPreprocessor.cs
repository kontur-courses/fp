using System.Linq;

namespace CTV.Common.Preprocessors
{
    public class ToLowerPreprocessor : IWordsPreprocessor
    {
        public string[] Preprocess(string[] rawWords)
        {
            return rawWords
                .Select(word => word.ToLower())
                .ToArray();
        }
    }
}