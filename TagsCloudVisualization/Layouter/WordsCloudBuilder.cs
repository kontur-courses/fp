using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudVisualization.WordsProcessing;

namespace TagsCloudVisualization.Layouter
{
    public class WordsCloudBuilder : IWordsCloudBuilder
    {
        private readonly ICloudLayouter layouter;
        private readonly IWordsProvider wordsProvider;
        private readonly ISizeConverter sizeConverter;
        private readonly IWeighter weighter;

        public int Radius => layouter.Radius;

        public WordsCloudBuilder(IWordsProvider wordsProvider, ICloudLayouter layouter, ISizeConverter sizeConverter, IWeighter weighter)
        {
            this.wordsProvider = wordsProvider;
            this.layouter = layouter;
            this.sizeConverter = sizeConverter;
            this.weighter = weighter;
        }

        public Result<IEnumerable<Word>> Build()
        {
            return wordsProvider
                .Provide()
                .Then(weighter.WeightWords)
                .Then(sizeConverter.Convert)
                .Then(PutWords);
        }

        private Result<IEnumerable<Word>> PutWords(IEnumerable<SizedWord> sizedWords)
        {
            var result = new List<Word>();
            foreach (var word in sizedWords)
            {
                var layResult = layouter.PutNextRectangle(word.Size);
                if (!layResult.IsSuccess)
                    return Result.Fail<IEnumerable<Word>>(layResult.Error);
                result.Add(new Word(word.Word, word.Font, layResult.Value));
            }
            return Result.Ok(result.AsEnumerable());
        }

    }
}
