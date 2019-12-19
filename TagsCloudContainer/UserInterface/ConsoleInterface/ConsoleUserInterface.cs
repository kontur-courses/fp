using Fclp;
using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Core;
using TagsCloudContainer.UserInterface.ArgumentsParsing;

namespace TagsCloudContainer.UserInterface.ConsoleInterface
{
    public class ConsoleUserInterface : IUserInterface, IResultDisplay
    {
        private readonly IArgumentsParser<UserInterfaceArguments> argumentsParser;

        public ConsoleUserInterface(IArgumentsParser<UserInterfaceArguments> argumentsParser)
        {
            this.argumentsParser = argumentsParser;
        }

        public void Run(string[] programArgs, Action<Parameters> runProgram)
        {
            ParseParameters(programArgs)
                .OnFail(ShowError)
                .Then(runProgram);
        }

        public void ShowResult(Bitmap bitmap)
        {
            Console.WriteLine(@"Successfully created tag cloud");
        }

        public void ShowError(string errorMessage)
        {
            Console.WriteLine($@"An error has occurred: {errorMessage}");
        }

        private Result<Parameters> ParseParameters(string[] programArgs)
        {
            var parser = SetupParser();

            var parseResult = parser.Parse(programArgs);
            if (!parseResult.HasErrors && !parseResult.HelpCalled)
            {
                return argumentsParser.ParseArgumentsToParameters(parser.Object);
            }

            return Result.Fail<Parameters>(parseResult.ErrorText);
        }

        private FluentCommandLineParser<UserInterfaceArguments> SetupParser()
        {
            var parser = new FluentCommandLineParser<UserInterfaceArguments>();

            parser.Setup(arg => arg.InputFilePath).As('i', "input").Required()
                .WithDescription("input file path (required)");
            parser.Setup(arg => arg.OutputFilePath).As('o', "output").SetDefault("test.png")
                .WithDescription("output file path, default is test.png");
            parser.Setup(arg => arg.Width).As('w', "width").SetDefault(800)
                .WithDescription("width of image, default is 800");
            parser.Setup(arg => arg.Height).As('h', "height").SetDefault(600)
                .WithDescription("height of image, default is 600");
            parser.Setup(arg => arg.Font).As('f', "font").SetDefault("Arial")
                .WithDescription("name of font, default is Arial");
            parser.Setup(arg => arg.Colors).As("colors").SetDefault(new List<string> { "Aqua", "Black" })
                .WithDescription("names of colors to use, default: Aqua Black");
            parser.Setup(arg => arg.ImageFormat).As('e', "extension").SetDefault("Png")
                .WithDescription("extension of image file, default is Png");

            parser.SetupHelp("?", "help").UseForEmptyArgs().Callback(text => Console.WriteLine(text))
                .WithHeader("Arguments to use:");

            return parser;
        }
    }
}