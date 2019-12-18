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

        public Result<bool> CreateImage(IInitialSettings settings)
        {
            var words = reader.ReadWords(settings.InputFilePath);
            var wordProcessorResult = wordProcessor.ProcessWords(words);
            if (!wordProcessorResult.IsSuccess)
                return Result.Fail<bool>(wordProcessorResult.Error);
            var wordsWithCount = wordProcessorResult.Value.ToList();
            if (wordsWithCount.Count == 0)
                return Result.Fail<bool>("Empty words list");
            var sizes = converter.ConvertToSizes(wordsWithCount);
            var rectangles = layouter.GetRectangles(sizes);
            var imageSize = imageSizeCalculator.CalculateImageSize(rectangles).Value;
            if (settings.ImageSize != Size.Empty)
            {
                if (settings.ImageSize.Width < imageSize.Width || settings.ImageSize.Height < imageSize.Height)
                    return Result.Fail<bool>("Tag cloud does not fit to size");
                imageSize = settings.ImageSize;
            }
            rectangles = rectanglesTransformer.TransformRectangles(rectangles, imageSize);
            words = wordsWithCount.Select(e => e.Word);
            var wordRectangles = words.Zip(rectangles, (w, r) => new WordRectangle(w, r)).ToList();
            var image = visualizer.DrawImage(wordRectangles, imageSize);
            saver.SaveImage(image, settings.OutputFilePath);
            return Result.Ok(true);
        }
    }
}
