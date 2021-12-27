using System.Drawing;
using System.IO;
using System.Linq;
using CTV.Common;
using CTV.Common.ImageSavers;
using CTV.Common.Preprocessors;
using CTV.Common.Readers;
using FunctionalProgrammingInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using CTV.Common.ImageSavers;

namespace CTV.ConsoleInterface
{
    public class ConsoleProcessor
    {
        public static Result<None> Render(VisualizerOptions options)
        {
            return
                from container in VisualizerContainerFactory.CreateInstance().AsResult()
                from fileStreamFactory in Result.Of(container.GetService<IFileStreamFactory>)
                from fileReader in Result.Of(() => options.InputFileFormat.ToFileReader())
                from wordsParser in Result.Of(container.GetService<IWordsParser>)
                from preprocessor in Result.Of(container.GetService<IWordsPreprocessor>)
                from visualizer in Result.Of(container.GetService<Visualizer>)
                from visualizerSetting in Result.Ok(GetVisualizerSetting(options))
                from imageSaver in Result.Of(() => options.SavingFormat.ToImageSaver())
                from inStream in fileStreamFactory.OpenOnReading(options.InputFile)
                from words in PrepareWordsToVisualize(inStream, fileReader, wordsParser, preprocessor)
                from image in visualizer.Visualize(words, visualizerSetting)
                from oStream in fileStreamFactory.OpenOnWriting(options.OutputFile)
                from _ in SaveImage(image, oStream, imageSaver)
                select _;
        }

        private static Result<string[]> PrepareWordsToVisualize(
            Stream inStream,
            IFileReader fileReader,
            IWordsParser wordsParser,
            IWordsPreprocessor preprocessor)
        {
            return
                Result.Ok(inStream)
                    .Then(fileReader.ReadToEnd)
                    .Then(wordsParser.Parse)
                    .Then(preprocessor.Preprocess);
        }

        private static Result<None> SaveImage(Bitmap image, Stream oStream, IImageSaver imageSaver)
        {
            return oStream
                .AsResult()
                .Then(stream => imageSaver.Save(image, stream))
                .InAnyCase(oStream.Close);
        }
     
        private static VisualizerSettings GetVisualizerSetting(VisualizerOptions options)
        {
            return new VisualizerSettings(
                options.ImageSize,
                options.Font,
                options.TextColor,
                options.BackgroundColor,
                options.StrokeColor
            );
        }
    }
}