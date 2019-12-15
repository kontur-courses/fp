using System;
using System.IO;
using TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands;
using TagsCloudResult.TextParsing;
using TagsCloudResult.TextParsing.CloudParsing.ParsingRules;
using TagsCloudResult.TextParsing.FileWordsParsers;

namespace TagsCloudResult.ApplicationRunning.Commands
{
    public class ParseCommand : IConsoleCommand
    {
        private readonly TagsCloud cloud;
        private readonly SettingsManager manager;
        private string path;

        public ParseCommand(TagsCloud cloud, SettingsManager manager)
        {
            this.cloud = cloud;
            this.manager = manager;
        }

        public Result<string[]> ParseArguments(string[] args)
        {
            return Check.ArgumentsCountIs(args, 1)
                .Then(CheckPath);
        }

        public Result<None> Act()
        {
            var extension = Path.GetExtension(path);
            var parser = WordsParser.GetParser(extension);
            Parse(parser, path);
            Console.WriteLine($"Successfully parsed words from: '{path}'");
            return Result.Ok();
        }

        public string Name => "Parse";
        public string Description => "Parse words from path";
        public string Arguments => "path";

        private Result<string[]> CheckPath(string[] args)
        {
            path = string.Join(" ", args).Trim('\'');
            var errorMessage = $"No file '{path}' found!";
            var checkRes = Check.Argument(path, errorMessage, File.Exists(path));
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private void Parse(IFileWordsParser parser, string path)
        {
            manager.ConfigureWordsParserSettings(parser, path, new DefaultParsingRule());
            cloud.ParseWords();
        }
    }
}