using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.WordsPreparator;

namespace TagsCloudContainer.WordsFilter
{
    public class LengthFilter : IWordsFilter
    {
        private readonly int minLength;


        public LengthFilter(ITagCloudSettings settings)
        {
            minLength = settings.MinWordLength;
        }

        public ICollection<WordInfo> Filter(ICollection<WordInfo> words)
        {
            return words
                .Where(word => word.Lemma.Length >= minLength)
                .ToArray();
        }
    }
}