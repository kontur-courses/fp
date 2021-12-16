using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Rendering;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer
{
    public class TagsCloudDirector : ITagsCloudDirector
    {
        private readonly IEnumerable<IWordsPreprocessor> preprocessors;
        private readonly IWordColorMapperSettings colorMapperSettings;
        private readonly ITagsCloudLayouter tagsCloudLayouter;
        private readonly ITagsCloudRenderer renderer;

        public TagsCloudDirector(
            IEnumerable<IWordsPreprocessor> preprocessors,
            IWordColorMapperSettings colorMapperSettings,
            ITagsCloudRenderer renderer,
            ITagsCloudLayouter tagsCloudLayouter)
        {
            this.preprocessors = preprocessors;
            this.colorMapperSettings = colorMapperSettings;
            this.renderer = renderer;
            this.tagsCloudLayouter = tagsCloudLayouter;
        }

        public Result<Bitmap> RenderWords(IEnumerable<string> words)
        {
            return PreprocessWords(words)
                .Then(tagsCloudLayouter.GetCloudLayout)
                .Then(layout => colorMapperSettings.ColorMapper.GetColorMap(layout)
                    .Then(colorMap => GetWordsStyles(layout, colorMap).ToList())
                    .Then(wordsStyles => CreateBitmap(wordsStyles, layout)));
        }

        private Bitmap CreateBitmap(List<WordStyle> wordsStyles, CloudLayout layout)
        {
            var bmp = renderer.GetBitmap(wordsStyles, layout.ImageSize);
            wordsStyles.ForEach(x => x.Dispose());
            return bmp;
        }

        private Result<IEnumerable<string>> PreprocessWords(IEnumerable<string> words)
        {
            return preprocessors.Aggregate(
                words.AsResult(),
                (result, preprocessor) => result.Then(preprocessor.Preprocess));
        }

        private static IEnumerable<WordStyle> GetWordsStyles(
            CloudLayout layout,
            IReadOnlyDictionary<WordLayout, Color> colorMap)
        {
            foreach (var wordLayout in layout.WordLayouts)
            {
                var brush = new SolidBrush(colorMap[wordLayout]);
                yield return new WordStyle(wordLayout.Word, wordLayout.Font, wordLayout.Location, brush);
            }
        }
    }
}