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

        public Result<CloudVisualization> GenerateCloud()
        {
            var words = ReadWords();
            if (!words.IsSuccess)
                return Result.Fail<CloudVisualization>(words.Error);

            words = PreprocessWords(words);
            if (!words.IsSuccess)
                return Result.Fail<CloudVisualization>(words.Error);

            words = FilterWords(words);
            if (!words.IsSuccess)
                return Result.Fail<CloudVisualization>(words.Error);

            var wordsFrequencies = frequencyAnalyzer.AnalyzeWordsFrequency(words.Value);
            var tags = tagger.CreateRawTags(wordsFrequencies).ToArray();

            var layouter = layouterFactory.CreateLayouter();
            if (!layouter.IsSuccess)
                return Result.Fail<CloudVisualization>(layouter.Error);

            var correctTags = new List<Tag>();
            foreach (var tag in tags)
            {
                var rectResult = layouter.Value.PutNextRectangle(tag.WordOuterRectangle.Size);
                if (rectResult.IsSuccess)
                {
                    tag.WordOuterRectangle = rectResult.Value;
                    correctTags.Add(tag);
                }
            }

            return GetCloudVisualization(layouter.Value, correctTags);
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

            if (!reader.IsSuccess)
            {
                var msg = $"{reader.Error}. Can not find suitable file reader";
                return Result.Fail<IEnumerable<string>>(msg);
            }

            var readResult = reader.Value.ReadLines();

            return readResult.IsSuccess
                ? readResult
                : Result.Fail<IEnumerable<string>>(readResult.Error);
        }

        private Result<CloudVisualization> GetCloudVisualization(ICloudLayouter layouter, IEnumerable<Tag> tags)
        {
            if (imageSizeSettings.Size.Width <= 0 || imageSizeSettings.Size.Height <= 0)
                return Result.Fail<CloudVisualization>("Incorrect image size.");

            var userRequiredSize = new Size(imageSizeSettings.Size.Width, imageSizeSettings.Size.Height);
            var cloudRequiredSize = layouter.GetRectanglesBoundaryBox();

            var bitmap = new Bitmap(imageSizeSettings.Size.Width, imageSizeSettings.Size.Height);
            var visualization = visualizer.VisualizeCloud(bitmap, layouter.Center, tags);

            return Result.Ok(new CloudVisualization(visualization, cloudRequiredSize, userRequiredSize));
        }
    }
}