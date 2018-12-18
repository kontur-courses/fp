using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Fclp;

namespace TagsCloudVisualization
{
    public class ConsoleApplication
    {
        private IFileReader fileReader;
        private IVisualizer visualizer;
        private IWordPalette wordPalette;
        private ISizeDefiner sizeDefiner;
        private ICloudLayouter cloudLayouter;
        private WordCounter counter = new WordCounter();
        private Size imageSize;
        private string fileName;

        public ConsoleApplication(IFileReader fileReader, IVisualizer visualizer, 
            IWordPalette wordPalette, ISizeDefiner sizeDefiner, ICloudLayouter cloudLayouter)
        {
            this.fileReader = fileReader;
            this.visualizer = visualizer;
            this.wordPalette = wordPalette;
            this.sizeDefiner = sizeDefiner;
            this.cloudLayouter = cloudLayouter;
        }

        public Result<FileSaveResult> GenerateImage(string[] args)
        {
            return Result.Ok(ParseArguments(args))
                .Then(ApplySettings)
                .Then(r => fileReader.Read(r.Path))
                .Then(counter.Count)
                .Then(LayoutWords)
                .Then(r => visualizer.Render(r, imageSize.Width, imageSize.Height, wordPalette))
                .Then(i => ImageSaver.WriteToFile(fileName, i));
        }

        private Result<CloudArguments> ApplySettings(CloudArguments arguments)
        {
            return Result.Ok(arguments)
                .Then(SetFont)
                .Then(SetImageSize)
                .Then(SetFileName);
        }

        private Result<IEnumerable<GraphicWord>> LayoutWords(IEnumerable<GraphicWord> words)
        {
            cloudLayouter.Process(words, sizeDefiner, GetImageCenter(imageSize.Width, imageSize.Height));
            return Result.Ok(words);
        }

        private CloudArguments ParseArguments(string[] args)
        {
            var parser = SetupParser();
            parser.Parse(args);

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
                counter.Font = new Font(arguments.Font, 8);
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
