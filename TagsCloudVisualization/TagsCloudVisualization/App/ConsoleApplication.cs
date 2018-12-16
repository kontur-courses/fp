using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        public ConsoleApplication(IFileReader fileReader, IVisualizer visualizer, 
            IWordPalette wordPalette, ISizeDefiner sizeDefiner, ICloudLayouter cloudLayouter)
        {
            this.fileReader = fileReader;
            this.visualizer = visualizer;
            this.wordPalette = wordPalette;
            this.sizeDefiner = sizeDefiner;
            this.cloudLayouter = cloudLayouter;
        }

        public void GenerateImage(string[] args)
        {
            Result.Ok(ParseArguments(args))
                .Then(SetFont)
                .Then(SetImageSize)
                .Then(r => fileReader.Read(r.FileName))
                .Then(counter.Count)
                .Then(LayoutWords)
                .Then(r => visualizer.Render(r, imageSize.Width, imageSize.Height, wordPalette))
                .Then(i => ImageSaver.WriteToFile("output.png", i));
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
            parser.Setup(arg => arg.FileName).As('p', "path").Required();
            parser.Setup(arg => arg.Width).As('w', "width").SetDefault(800);
            parser.Setup(arg => arg.Height).As('h', "height").SetDefault(800);
            parser.Setup(arg => arg.Font).As('f', "font").SetDefault("Arial");

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
            imageSize = new Size(arguments.Width, arguments.Height);
            return Result.Ok(arguments);
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
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);
        }

        private Point GetImageCenter(int width, int height)
        {
            return new Point(width / 2, height / 2);
        }
    }
}
