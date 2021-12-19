using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Visualizer;
using TagsCloudContainer.WordsFilter;
using TagsCloudContainer.WordsFrequencyAnalyzers;
using TagsCloudContainer.WordsPreparator;

namespace TagsCloudContainer
{
    public class TagCloud : ITagCloud
    {
        private readonly IFilterApplyer filterApplyer;
        private readonly IWordsFrequencyAnalyzer frequencyAnalyzer;
        private readonly IVisualizer visualizer;
        private readonly IWordsPreparator wordsConverter;


        public TagCloud(IFilterApplyer filterApplyer,
            IWordsFrequencyAnalyzer frequencyAnalyzer,
            IVisualizer visualizer,
            IWordsPreparator wordsConverter)
        {
            this.filterApplyer = filterApplyer;
            this.frequencyAnalyzer = frequencyAnalyzer;
            this.visualizer = visualizer;
            this.wordsConverter = wordsConverter;
        }

        public Result<Bitmap> LayDown(IEnumerable<string> words)
        {
            return Result.Of(() => wordsConverter.Prepare(words))
                .Then(convertedWords => filterApplyer.Apply(convertedWords))
                .Then(filteredWords => frequencyAnalyzer.GetWordsFrequency(filteredWords))
                .Then(freqDict => visualizer.Visualize(freqDict))
                .RefineError("Произошла ошибка при формировании облака тегов: ");
        }
    }
}