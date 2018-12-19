using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Fclp;
using TagsCloudVisualization.App.Cloud.Words;

namespace TagsCloudVisualization
{
    public class ConsoleApplication
    {
        private IFileReader fileReader;
        private IVisualizer visualizer;
        private IWordPalette wordPalette;
        private ISizeDefiner sizeDefiner;
        private ICloudLayouter cloudLayouter;
        private IWordCounter wordCounter;
        private IImageSaver imageSaver;
        private Size imageSize;
        private string fileName;

        public ConsoleApplication(IFileReader fileReader, IVisualizer visualizer, IWordPalette wordPalette, 
            ISizeDefiner sizeDefiner, ICloudLayouter cloudLayouter, IWordCounter wordCounter, IImageSaver imageSaver)
        {
            this.fileReader = fileReader;
            this.visualizer = visualizer;
            this.wordPalette = wordPalette;
            this.sizeDefiner = sizeDefiner;
            this.cloudLayouter = cloudLayouter;
            this.wordCounter = wordCounter;
            this.imageSaver = imageSaver;
        }

        public Result<FileSaveResult> GenerateImage(string[] args)
        {
            return ParseArguments(args)
                .Then(ApplySettings)
                .Then(r => fileReader.Read(r.Path))
                .Then(wordCounter.Count)
                .Then(LayoutWords)
                .Then(ValidateImageSize)
                .Then(r => visualizer.Render(r, imageSize.Width, imageSize.Height, wordPalette))
                .Then(i => imageSaver.WriteToFile(fileName, i));
        }

        private Result<CloudArguments> ApplySettings(CloudArguments arguments)
        {
            return Result.Ok(arguments)
                .Then(SetFont)
                .Then(SetImageSize)
                .Then(SetFileName);
        }

        private Result<List<GraphicWord>> LayoutWords(List<GraphicWord> words)
        {
            cloudLayouter.Process(words, sizeDefiner, GetImageCenter(imageSize.Width, imageSize.Height));
            return words;
        }

        private Result<CloudArguments> ParseArguments(string[] args)
        {
            var parser = SetupParser();
            var result = parser.Parse(args);

            if (result.HasErrors)
                return Result.Fail<CloudArguments>(result.ErrorText);

            var settings = parser.Object;

            return settings;
        }

        private FluentCommandLineParser<CloudArguments> SetupParser()
        {
            var parser = new FluentCommandLineParser<CloudArguments>();
            parser.Setup(arg => arg.Path).As('p', "path").Required();
            parser.Setup(arg => arg.Width).As('w', "width").SetDefault(800);
            parser.Setup(arg => arg.Height).As('h', "height").SetDefault(800);
            parser.Setup(arg => arg.Font).As('f', "font").SetDefault("Arial");
            parser.Setup(arg => arg.FileName).As('n', "name").SetDefault("output.png");

            return parser;
        }

        private Result<CloudArguments> SetFont(CloudArguments arguments)
        {
            var result = ValidateFontName(arguments);
            if (result.IsSuccess)
            {
                wordCounter.Font = new Font(arguments.Font, 8);
            }

            return result;
        }

        private Result<CloudArguments> SetImageSize(CloudArguments arguments)
        {
            if (arguments.Width <= 0 || arguments.Height <= 0)
                return Result.Fail<CloudArguments>("Image size must be positive?");
            imageSize = new Size(arguments.Width, arguments.Height);
            return arguments;
        }

        private Result<CloudArguments> SetFileName(CloudArguments arguments)
        {
            fileName = arguments.FileName;
            return arguments;
        }

        private Result<List<GraphicWord>> ValidateImageSize(List<GraphicWord> words)
        {
            foreach (var graphicWord in words)
            {
                if (Geometry.GetVertices(graphicWord.Rectangle)
                    .Any(vertex => vertex.X > imageSize.Width || vertex.X < 0 || 
                                   vertex.Y > imageSize.Height || vertex.Y < 0))
                    return Result.Fail<List<GraphicWord>>("Cloud is out of image, try set image size bigger");
            }

            return words;
        }

        private Result<CloudArguments> ValidateFontName(CloudArguments arguments)
        {
            var font = new Font(arguments.Font, 8);
            return Validate(arguments,
                f => arguments.Font.Equals(font.Name, 
                    StringComparison.InvariantCultureIgnoreCase),
                $"Invalid font name: {arguments.Font}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? obj
                : Result.Fail<T>(errorMessage);
        }

        private Point GetImageCenter(int width, int height)
        {
            return new Point(width / 2, height / 2);
        }
    }
}
