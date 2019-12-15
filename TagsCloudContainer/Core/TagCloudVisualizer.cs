using System.Drawing;
using ResultOf;
using TagsCloudContainer.Algorithm;
using TagsCloudContainer.FileReading;
using TagsCloudContainer.Visualizing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.Core
{
    public class TagCloudVisualizer : ITagCloudVisualizer
    {
        private readonly IFileReader fileReader;
        private readonly IWordProcessor wordProcessor;
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly IVisualizer visualizer;

        public TagCloudVisualizer(IFileReader fileReader, IWordProcessor wordProcessor,
            ILayoutAlgorithm layoutAlgorithm, IVisualizer visualizer)
        {
            this.fileReader = fileReader;
            this.wordProcessor = wordProcessor;
            this.layoutAlgorithm = layoutAlgorithm;
            this.visualizer = visualizer;
        }

        public Result<Bitmap> GetTagCloudBitmap(Parameters parameters)
        {
            return Result.Of(() => fileReader.ReadWords(parameters.InputFilePath))
                .Then(words => wordProcessor.ProcessWords(words))
                .Then(processedWords => layoutAlgorithm.GetLayout(processedWords, parameters.ImageSize))
                .Then(layout =>
                    visualizer.GetLayoutBitmap(layout, parameters.Font, parameters.ImageSize, parameters.Colors));
            //var words = fileReader.ReadWords(parameters.InputFilePath);
            //var processedWords = wordProcessor.ProcessWords(words);
            //var layout = layoutAlgorithm.GetLayout(processedWords, parameters.ImageSize);
            //return visualizer.GetLayoutBitmap(layout, parameters.Font, parameters.ImageSize, parameters.Colors);
        }
    }
}