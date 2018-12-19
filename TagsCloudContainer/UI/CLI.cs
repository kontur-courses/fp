using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using CommandLine;
using ResultOf;
using TagsCloudContainer.Visualisation;

namespace TagsCloudContainer.UI
{
    public class CLI : IUI
    {
        public ApplicationSettings ApplicationSettings { get; }

        private Result<string> InputPath { get; set; }
        private string OutputPath { get; set; }
        private Result<string> BlacklistPath { get; set; }
        private Result<Point> TagsCloudCenter { get; set; }
        private Result<Size> LetterSize { get; set; }
        private Result<Color> TextColor { get; set; }
        private Result<Size> ImageSize { get; set; }
        private FontFamily FontFamily { get; }
        private bool AutoSize { get; set; }

        public CLI(string[] args)

        {
            InputPath = AppDomain.CurrentDomain.BaseDirectory + "\\cloud.docx";
            OutputPath = "output.png";
            BlacklistPath = "blacklist.txt";
            ImageSize = new Size(1920, 1080);
            TagsCloudCenter = ImageSize.Then(x => new Point(x.Width / 2, x.Height / 2));
            TextColor = Color.DarkBlue;
            LetterSize = new Size(16, 20);
            FontFamily = FontFamily.GenericMonospace;
            AutoSize = true;


            ParseArguments(args);
            var checkingResult = CheckArguments();
            if (!checkingResult.IsSuccess)
            {
                Console.WriteLine(checkingResult.Error);
                Environment.Exit(0);
            }

            ApplicationSettings = new ApplicationSettings
            (GetValue(InputPath), GetValue(BlacklistPath), GetValue(TagsCloudCenter),
                new ImageSettings
                    (FontFamily, GetValue(ImageSize), GetValue(LetterSize), OutputPath, GetValue(TextColor), AutoSize));
        }

        private T GetValue<T>(Result<T> result)
        {
            var value = default(T);
            result.Then(x => value = x);
            return value;
        }

        private Result<None> CheckArguments()
        {
            var errors = new List<string>()
            {
                TagsCloudCenter.Error, LetterSize.Error, TextColor.Error,
                ImageSize.Error, InputPath.Error, BlacklistPath.Error
            };
            foreach (var error in errors)
            {
                if (error != null)
                {
                    var result = Result.Fail<None>(error);
                    result.RefineError("Parsing arguments error");
                    return result;
                }
            }

            return new Result<None>();
        }

        private class Options
        {
            [Option('i', "input", Required = false, HelpText = "Set input file path.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = false, HelpText = "Set output file path.")]
            public string OutputPath { get; set; }

            [Option('b', "blacklist", Required = false, HelpText = "Set words blacklist file path."
            )]
            public string BlackListPath { get; set; }

            [Option('c', "center", Required = false, HelpText = "Set tags cloud center.")]
            public IEnumerable<string> TagsCloudCenter { get; set; }

            [Option('l', "lettersize", Required = false, HelpText = "Set minimum letter size.")]
            public IEnumerable<string> LetterSize { get; set; }

            [Option('p', "picturesize", Required = false, HelpText = "Set output image size.")]
            public IEnumerable<string> ImageSize { get; set; }

            [Option('t', "textcolor", Required = false,
                HelpText = "Set text color.")]
            public string TextColor { get; set; }

            [Option('a', "autosize", Required = false,
                HelpText = "Disable auto sizing")]
            public bool AutoSize { get; set; }
        }

        private void ParseArguments(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    if (o.InputPath != null)
                        InputPath = CheckFileExistence(o.InputPath);

                    if (o.BlackListPath != null)
                        BlacklistPath = CheckFileExistence(o.BlackListPath);

                    if (o.OutputPath != null)
                        OutputPath = o.OutputPath;

                    if (o.TagsCloudCenter.Any())
                    {
                        var name = "center";
                        var center = new ReadOnlyCollection<string>(o.TagsCloudCenter.ToList());
                        TagsCloudCenter = ValidateArgumentsCount(center, 2, name)
                            .Then(x => ParseNumbers(center, name))
                            .Then(x => ValidatePositiveNumbers(x, name))
                            .Then(x => new Point(x.ToArray()[0], x.ToArray()[1]));
                    }


                    if (o.LetterSize.Any())
                    {
                        var name = "letter size";
                        var letterSize = new ReadOnlyCollection<string>(o.LetterSize.ToList());
                        LetterSize = ValidateArgumentsCount(letterSize, 2, name)
                            .Then(x => ParseNumbers(letterSize, name))
                            .Then(x => ValidatePositiveNumbers(x, name))
                            .Then(x => new Size(x.ToArray()[0], x.ToArray()[1]));
                    }

                    if (o.TextColor != null)
                    {
                        var color = Color.FromName(o.TextColor);
                        TextColor = !color.IsKnownColor
                            ? Result.Fail<Color>($"Color '{o.TextColor}' is unknown")
                            : Result.Ok(color);
                    }


                    if (o.ImageSize.Any())
                    {
                        var imageSize = new ReadOnlyCollection<string>(o.ImageSize.ToList());
                        var name = "image size";
                        ImageSize = ValidateArgumentsCount(imageSize, 2, name)
                            .Then(x => ParseNumbers(x, name))
                            .Then(x => ValidatePositiveNumbers(x, name))
                            .Then(x => new Size(x.ToArray()[0], x.ToArray()[1]));
                    }

                    if (o.AutoSize)
                    {
                        AutoSize = false;
                    }
                });
        }

        private Result<ReadOnlyCollection<int>> ParseNumbers(ReadOnlyCollection<string> input, string argName)
        {
            return Result.Of(() => new ReadOnlyCollection<int>(input.Select(int.Parse).ToList()),
                $"Entered Argument '{argName}' value '{string.Join(" ", input)}' contains not number values");
        }

        private Result<ReadOnlyCollection<string>> ValidateArgumentsCount(ReadOnlyCollection<string> input, int count,
            string argName)
        {
            if (input.Count() == count)
            {
                return Result.Ok((input));
            }

            return Result.Fail<ReadOnlyCollection<string>>(
                $"Argument '{argName}' should contain {count} numbers, but was {input.Count()} (Entered value: '{string.Join(" ", input)}')");
        }

        private Result<ReadOnlyCollection<int>> ValidatePositiveNumbers(ReadOnlyCollection<int> input, string argName)
        {
            return input.Any(x => x <= 0)
                ? Result.Fail<ReadOnlyCollection<int>>
                    ($"Argument '{argName}' should contain only positive numbers (Entered value: '{string.Join(" ", input)}')")
                : Result.Ok(input);
        }

        private Result<string> CheckFileExistence(string path)
        {
            return File.Exists(path) ? Result.Ok(path) : Result.Fail<string>($"File {path} not found");
        }
    }
}