using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.ImageSaver;
using TagsCloudContainer.ImageSizeCalculator;
using TagsCloudContainer.Layouter;
using TagsCloudContainer.Reader;
using TagsCloudContainer.RectanglesTransformer;
using TagsCloudContainer.UI;
using TagsCloudContainer.UI.SettingsCommands;
using TagsCloudContainer.Visualizer;
using TagsCloudContainer.WordProcessor;
using TagsCloudContainer.WordsToSizesConverter;

namespace TagsCloudContainer.ImageCreator
{
    public class ImageCreator : IImageCreator
    {
        private readonly ITextReader reader;
        private readonly IWordProcessor wordProcessor;
        private readonly IWordFrequenciesToSizesConverter converter;
        private readonly ILayouter layouter;
        private readonly IImageSizeCalculator imageSizeCalculator;
        private readonly IRectanglesTransformer rectanglesTransformer;
        private readonly IVisualizer visualizer;
        private readonly IImageSaver saver;

        public ImageCreator(ITextReader reader, IWordProcessor wordProcessor,
            IWordFrequenciesToSizesConverter converter, ILayouter layouter, IImageSizeCalculator imageSizeCalculator,
            IRectanglesTransformer rectanglesTransformer, IVisualizer visualizer, IImageSaver saver)
        {
            this.reader = reader;
            this.wordProcessor = wordProcessor;
            this.converter = converter;
            this.layouter = layouter;
            this.imageSizeCalculator = imageSizeCalculator;
            this.rectanglesTransformer = rectanglesTransformer;
            this.visualizer = visualizer;
            this.saver = saver;
        }

        private static Result<Size> HandleSize(IInitialSettings settings, Size calculatedSize)
        {
            if (settings.ImageSize == Size.Empty)
                return calculatedSize;
            if (settings.ImageSize.Width < calculatedSize.Width ||
                settings.ImageSize.Height < calculatedSize.Height)
                return Result.Fail<Size>("Tag cloud does not fit to size");
            return settings.ImageSize;
        }

        private List<WordRectangle> GetWordRectangles(IEnumerable<string> words, IEnumerable<Rectangle> rectangles, Size imageSize)
        {
            var transformedRectangles = rectanglesTransformer.TransformRectangles(rectangles, imageSize);
            return words.Zip(transformedRectangles, (w, r) => new WordRectangle(w, r)).ToList();
        }

        public Result<None> CreateImage(IInitialSettings settings)
        {
            var words = reader.ReadWords(settings.InputFilePath);
            return wordProcessor.ProcessWords(words)
                .Then(wordsWithCount => wordsWithCount.ToList())
                .Then(wordsWithCount =>
                {
                    words = wordsWithCount.Select(e => e.Word);
                    var sizes = converter.ConvertToSizes(wordsWithCount);
                    var rectangles = layouter.GetRectangles(sizes);
                    return imageSizeCalculator.CalculateImageSize(rectangles)
                        .Then(calculatedSize => HandleSize(settings, calculatedSize))
                        .Then(imageSize => (rect: GetWordRectangles(words, rectangles, imageSize), imageSize));
                })
                .Then(x => visualizer.DrawImage(x.rect, x.imageSize))
                .Then(image => saver.SaveImage(image, settings.OutputFilePath));
        }
    }
}
