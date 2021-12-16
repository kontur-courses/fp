using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using App.Implementation.Words.Tags;
using App.Infrastructure;
using App.Infrastructure.FileInteractions.Readers;
using App.Infrastructure.LayoutingAlgorithms;
using App.Infrastructure.LayoutingAlgorithms.AlgorithmFromTDD;
using App.Infrastructure.SettingsHolders;
using App.Infrastructure.Visualization;
using App.Infrastructure.Words.Filters;
using App.Infrastructure.Words.FrequencyAnalyzers;
using App.Infrastructure.Words.Preprocessors;
using App.Infrastructure.Words.Tags;

namespace App.Implementation
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly IEnumerable<IFilter> filters;
        private readonly IFrequencyAnalyzer frequencyAnalyzer;
        private readonly IImageSizeSettingsHolder imageSizeSettings;
        private readonly ILayouterFactory layouterFactory;
        private readonly IEnumerable<IPreprocessor> preprocessors;
        private readonly IReaderFactory readerFactory;
        private readonly ITagger tagger;
        private readonly IVisualizer visualizer;

        public CloudGenerator(
            IReaderFactory readerFactory,
            ITagger tagger,
            IFrequencyAnalyzer frequencyAnalyzer,
            IVisualizer visualizer,
            ILayouterFactory layouterFactory,
            IImageSizeSettingsHolder imageSizeSettings,
            IEnumerable<IPreprocessor> preprocessors,
            IEnumerable<IFilter> filters)
        {
            this.readerFactory = readerFactory;
            this.tagger = tagger;
            this.layouterFactory = layouterFactory;
            this.frequencyAnalyzer = frequencyAnalyzer;
            this.visualizer = visualizer;
            this.preprocessors = preprocessors;
            this.filters = filters;
            this.imageSizeSettings = imageSizeSettings;
        }

        public Result<Bitmap> GenerateCloud()
        {
            var words = ReadWords();

            words = PreprocessWords(words);
            words = FilterWords(words);

            var wordsFrequencies = frequencyAnalyzer.AnalyzeWordsFrequency(words.Value);
            var tags = tagger.CreateRawTags(wordsFrequencies).ToArray();

            var layouter = layouterFactory.CreateLayouter();

            foreach (var tag in tags)
            {
                var rectResult = layouter.PutNextRectangle(tag.WordOuterRectangle.Size);
                if (rectResult.IsSuccess)
                    tag.WordOuterRectangle = rectResult.Value;
            }

            return GetBitmap(layouter, tags);
        }

        private Result<IEnumerable<string>> PreprocessWords(Result<IEnumerable<string>> words)
        {
            if (!words.IsSuccess)
                return words;

            foreach (var preprocessor in preprocessors)
            {
                words = preprocessor.Preprocess(words);
                if (!words.IsSuccess)
                {
                    var msg = $"Error when preprocessing words. {words.Error}";
                    return Result.Fail<IEnumerable<string>>(msg);
                }
            }

            return words;
        }

        private Result<IEnumerable<string>> FilterWords(Result<IEnumerable<string>> words)
        {
            if (!words.IsSuccess)
                return words;

            foreach (var filter in filters)
            {
                words = filter.FilterWords(words);
                if (!words.IsSuccess)
                {
                    var msg = $"Error when Filtering words. {words.Error}";
                    return Result.Fail<IEnumerable<string>>(msg);
                }
            }

            return words;
        }

        private Result<IEnumerable<string>> ReadWords()
        {
            var reader = readerFactory.CreateReader();

            return reader.IsSuccess
                ? reader.Value.ReadLines()
                : Result.Fail<IEnumerable<string>>(
                    $"{reader.Error}. Can not find suitable file reader");
        }

        private Result<Bitmap> GetBitmap(ICloudLayouter layouter, IEnumerable<Tag> tags)
        {
            var bitmap = new Bitmap(imageSizeSettings.Size.Width, imageSizeSettings.Size.Height);
            return Result.Ok(visualizer.VisualizeCloud(bitmap, layouter.Center, tags));
        }
    }
}