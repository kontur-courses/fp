using CommandLine;
using ResultOf;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.Arguments
{
    public class ArgumentsParser
    {
        public Result<string> InputPath { get; private set; }
        public Result<string> OutputPath { get; private set; }
        public Result<string> WordsToExcludePath { get; private set; }
        public Result<string> FontName { get; private set; }
        public Result<Brush> Brush { get; private set; }

        private Dictionary<string, Brush> brushes = new Dictionary<string, Brush>()
        {
            {"red", Brushes.Red},
            {"green", Brushes.Green},
            {"blue", Brushes.Blue},
            {"black", Brushes.Black},
        };

        public ArgumentsParser(string[] args)
        {
            Parse(args);
        }

        private class Options
        {
            [Option('i', "input", Required = true, Default = "./input/input.txt", HelpText = "Set input file path.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, Default = "./output/output.jpg", HelpText = "Set output file path.")]
            public string OutputPath { get; set; }

            [Option('f', "font-name", Required = false, Default = "Arial", HelpText = "Set font name.")]
            public string FontName { get; set; }

            [Option('c', "color", Required = false, Default = "black",
                HelpText = "Set color")]
            public string Color { get; set; }

            [Option("words-to-exclude", Required = false, HelpText = "Set words to exclude file path.")]
            public string WordsToExclude { get; set; }
        }

        private void Parse(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.InputPath == null)
                        InputPath = Result.Fail<string>("Input path is null");
                    else
                        InputPath = Result.Ok(o.InputPath);
                    if (o.OutputPath == "")
                        OutputPath = Result.Fail<string>("Output path is null");
                    else
                        OutputPath = Result.Ok(o.OutputPath);
                    FontName = Result.Ok(o.FontName);

                    if (brushes.TryGetValue(o.Color, out var color))
                        Brush = color;
                    else
                        Brush = Brushes.Black;
                    
                        WordsToExcludePath = o.WordsToExclude;
                });
        }
    }
}
