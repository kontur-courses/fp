using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagCloud.CloudLayouters;
using TagCloud.TagCloudVisualizations;
using TagCloud.Tags;
using TagCloud.WordPreprocessors;

namespace TagCloud.TagCloudCreators
{
    public class WordTagCloudCreator : ITagCloudCreator
    {
        private readonly ICloudLayouter.Factory cloudLayouterFactory;
        private readonly IWordPreprocessor wordPreprocessor;
        private readonly ITagCloudVisualizationSettings settings;

        private ICloudLayouter cloudLayouter;

        public WordTagCloudCreator(
            ICloudLayouter.Factory cloudLayouterFactory, 
            IWordPreprocessor wordPreprocessor,
            ITagCloudVisualizationSettings settings)
        {
            this.cloudLayouterFactory = cloudLayouterFactory;
            this.wordPreprocessor = wordPreprocessor;
            this.settings = settings;
        }

        public Result<ITagCloud> GenerateTagCloud() =>
            PrepareWords().Then(PrepareTagCloud);

        private Result<IOrderedEnumerable<KeyValuePair<string, int>>> PrepareWords()
        {
            cloudLayouter = cloudLayouterFactory.Invoke();
            var processedWords = wordPreprocessor.GetPreprocessedWords();
            return !processedWords.IsSuccess
                ? Result.Fail<IOrderedEnumerable<KeyValuePair<string, int>>>(processedWords.Error)
                : processedWords.Value.GroupBy(word => word).
                Select(group => new KeyValuePair<string, int>(group.Key, group.Count())).
                OrderByDescending(group => group.Value).AsResult();
        }

        private Result<ITagCloud> PrepareTagCloud(IOrderedEnumerable<KeyValuePair<string, int>> wordsWithRate)
        {
            var installedFonts = new InstalledFontCollection();

            if (installedFonts.Families.All(font => font.Name != settings.FontFamilyName))
                return Result.Fail<ITagCloud>($"font {settings.FontFamilyName} not found");

            if(settings.TextScale < 1)
                return Result.Fail<ITagCloud>($"TextScale must be higher than 0");

            var tagCloud = new TagCloud(cloudLayouter.Center);
            foreach (var word in wordsWithRate)
            {
                var font = new Font(settings.FontFamilyName, GetFontSize(word.Value, settings.TextScale));
                tagCloud.Layouts.Add(new Word(word.Key, font, cloudLayouter));
            }
            return tagCloud;
        }

        private uint GetFontSize(int wordRate, uint scale) =>
            (uint)Math.Pow(wordRate, 0.5) * scale;
    }
}
