using System;
using System.Linq;

namespace CTV.Common.Preprocessors
{
    public class RemovingBoringWordsPreprocessor : IWordsPreprocessor
    {
        private readonly IHunspeller hunspeller;

        public RemovingBoringWordsPreprocessor(IHunspeller hunspeller)
        {
            this.hunspeller = hunspeller ?? throw new ArgumentNullException(nameof(hunspeller));
        }

        public string[] Preprocess(string[] rawWords)
        {
            return rawWords
                .Where(word => hunspeller.Check(word))
                .ToArray();
        }
    }
}