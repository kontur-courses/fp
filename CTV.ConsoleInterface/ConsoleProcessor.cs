using CTV.Common;
using CTV.Common.ImageSavers;
using CTV.Common.Preprocessors;
using CTV.Common.Readers;
using CTV.Common.VisualizerContainer;
using Microsoft.Extensions.DependencyInjection;

namespace CTV.ConsoleInterface
{
    public class ConsoleProcessor
    {
        public void Run(VisualizerOptions options)
        {
            var container = GetDIContainer(options);
            var fileStreamFactory = container.GetService<IFileStreamFactory>();
            var fileReader = container.GetService<IFileReader>();
            var wordsParser = container.GetService<IWordsParser>();
            var preprocessor = container.GetService<IWordsPreprocessor>();
            var visualizer = container.GetService<Visualizer>();
            var imageSaver = container.GetService<IImageSaver>();
            
            using var inStream = fileStreamFactory.OpenOnReading(options.PathToFileWithWords);
            
            var content = fileReader.ReadToEnd(inStream);
            var words = wordsParser.Parse(content);
            var preprocessesWords = preprocessor.Preprocess(words);

            var visualizedImage = visualizer.Visualize(preprocessesWords);

            using var oStream = fileStreamFactory.OpenOnWriting(options.PathToSaveImage);
            imageSaver.Save(visualizedImage, oStream);
        }

        public ServiceProvider GetDIContainer(VisualizerOptions options)
        {
            var factorySettings = new VisualizerFactorySettings
            {
                BackgroundColor = options.BackgroundColor,
                StrokeColor = options.StrokeColor,
                TextColor = options.TextColor,
                ImageSize = options.ImageSize,
                TextFont = options.Font,
                SavingFormat = options.SavingFormat,
                InputFileFormat = options.InputFileFormat
            };

            return VisualizerContainerFactory.CreateInstance(factorySettings);
        }
    }
}