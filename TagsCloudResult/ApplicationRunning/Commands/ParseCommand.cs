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
        private TagsCloud cloud;
        private SettingsManager manager;
        public ParseCommand(TagsCloud cloud, SettingsManager manager)
        {
            this.cloud = cloud;
            this.manager = manager;
        }
        public void Act(string[] args)
        {
            var path = string.Join(" ", args).Trim('\'');
            Check.Argument(path, $"No file '{path}' found!", File.Exists(path));
            var extension = Path.GetExtension(path);
            var parser = WordsParser.GetParser(extension);
            Parse(parser, path);
            Console.WriteLine($"Successfully parsed words from: '{path}'");
        }

        private void Parse(IFileWordsParser parser, string path)
        {
            manager.ConfigureWordsParserSettings(parser, path, new DefaultParsingRule());
            cloud.ParseWords();
        }

        public string Name => "Parse";
        public string Description => "Parse words from path";
        public string Arguments => "path";
    }
}