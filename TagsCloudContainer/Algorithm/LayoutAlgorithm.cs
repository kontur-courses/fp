using ResultOf;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Algorithm.Layouting;
using TagsCloudContainer.Algorithm.Organizing;
using TagsCloudContainer.Algorithm.SizeProviding;
using TagsCloudContainer.Algorithm.WeightSetting;

namespace TagsCloudContainer.Algorithm
{
    public class LayoutAlgorithm : ILayoutAlgorithm
    {
        private readonly IWordWeightSetter wordWeightSetter;
        private readonly IWordSizeProvider wordSizeProvider;
        private readonly IWordOrganizer wordOrganizer;
        private readonly ILayouter layouter;

        public LayoutAlgorithm(IWordWeightSetter wordWeightSetter, IWordSizeProvider wordSizeProvider,
            IWordOrganizer wordOrganizer, ILayouter layouter)
        {
            this.wordWeightSetter = wordWeightSetter;
            this.wordSizeProvider = wordSizeProvider;
            this.wordOrganizer = wordOrganizer;
            this.layouter = layouter;
        }

        public Result<IEnumerable<(string, Rectangle)>> GetLayout(IEnumerable<string> words, Size pictureSize)
        {
            return words.Select(w => new Word { Value = w }).AsResult()
                .Then(convertedWords => wordWeightSetter.SetWordsWeights(convertedWords))
                .Then(weightedWords => wordSizeProvider.SetWordsSizes(weightedWords, pictureSize))
                .Then(wordsWithSize => wordsWithSize.GroupBy(w => w.Value).Select(g => g.First()))
                .Then(wordsWithoutDuplicates => wordOrganizer.GetSortedWords(wordsWithoutDuplicates))
                .Then(orderedWords => layouter.GetWordsRectangles(orderedWords, pictureSize));
        }
    }
}