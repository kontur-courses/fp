using System.Drawing;
using System.IO;
using CTV.Common;
using CTV.Common.ImageSavers;
using CTV.Common.Preprocessors;
using CTV.Common.Readers;
using CTV.Common.VisualizerContainer;
using FunctionalProgrammingInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CTV.ConsoleInterface
{
    public class ConsoleProcessor
    {
        public Result<None> Run(VisualizerOptions options)
        {
            //Правильно ли здесь будет все эти интерфейсы вынести в поля?
            //Это вроде красиво будет, но в функциональном стиле не любят поля делать
            return Result.Ok()
                .InitVariable(() => GetDIContainer(options), out var container)
                .InitVariable(container.GetService<IFileStreamFactory>, out var fileStreamFactory)
                .InitVariable(container.GetService<IFileReader>, out var fileReader)
                .InitVariable(container.GetService<IWordsParser>, out var wordsParser)
                .InitVariable(container.GetService<IWordsPreprocessor>, out var preprocessor)
                .InitVariable(container.GetService<Visualizer>, out var visualizer)
                .InitVariable(container.GetService<IImageSaver>, out var imageSaver)
                .InitVariable(() => fileStreamFactory.OpenOnReading(options.InputFile), out Stream inStream)
                .Then(_ => PrepareWordsToVisualize(inStream, fileReader, wordsParser, preprocessor))
                .Then(words => visualizer.Visualize(words))
                .InitVariable(() => fileStreamFactory.OpenOnWriting(options.OutputFile), out Stream oStream)
                .Then(image => SaveImage(image, oStream, imageSaver));
        }

        private Result<string[]> PrepareWordsToVisualize(
            Stream inStream,
            IFileReader fileReader,
            IWordsParser wordsParser,
            IWordsPreprocessor preprocessor)
        {
            return inStream
                .AsResult()
                .Then(fileReader.ReadToEnd)
                .Then(wordsParser.Parse)
                .Then(preprocessor.Preprocess)
                .InAnyCase(inStream.Close);
        }

        private Result<None> SaveImage(Bitmap image, Stream oStream, IImageSaver imageSaver)
        {
            return oStream
                .AsResult()
                .Then(stream => imageSaver.Save(image, stream))
                .InAnyCase(oStream.Close);
        }

        private ServiceProvider GetDIContainer(VisualizerOptions options)
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