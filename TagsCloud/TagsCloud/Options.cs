using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using CommandLine;
using TagsCloud.ErrorHandler;
using TagsCloud.Visualization.ColorDefiner;
using TagsCloud.Visualization.SizeDefiner;

namespace TagsCloud
{
    public class Options
    {
        private static readonly List<Checker> Checkups = new List<Checker>
        {
            new Checker(o => File.Exists(o.FilePath), "File not found"),
            new Checker(o => Application.ImageFormatDenotation.ContainsKey(o.Format), "Wrong image format"),
            new Checker(o => o.MinFontSize > 0, "Minimum font size should be greater than 0."),
            new Checker(o => o.MaxFontSize > 0, "Maximum font size should be greater than 0."),
            new Checker(o => o.MaxFontSize >= o.MinFontSize,
                "Maximum font size should be greater or equal to minimum font size."),
            new Checker(o =>
            {
                var color = Color.FromName(o.BackgroundColor);
                return color.IsKnownColor;
            }, "Wrong background color name."),
            new Checker(o =>
            {
                var font = new Font(o.Font, 10);
                return o.Font == font.Name;
            }, "Wrong background color name.")
        };


        [Option('f', "file", Required = false, Default = @"in1.txt",
            HelpText = "The file from which we take the words")]
        public string FilePath { get; set; }

        [Option("font", Required = false, Default = "Arial", HelpText = "Defines font of words.")]
        public string Font { get; set; }

        [Option("minFS", Required = false, Default = 10, HelpText = "Min font-size.")]
        public int MinFontSize { get; set; }

        [Option("maxFS", Required = false, Default = 100, HelpText = "Max font-size.")]
        public int MaxFontSize { get; set; }

        [Option("bgcolor", Required = false, Default = "Black", HelpText = "Color of the background.")]
        public string BackgroundColor { get; set; }

        [Option("tagscolor", Required = false, Default = ColorDefinersType.Random,
            HelpText = "Color of tags" +
                       "Possible types are: Random")]
        public ColorDefinersType ColorDefiner { get; set; }


        [Option("sizedefiner", Required = false, Default = SizeDefinersType.Frequency, HelpText =
            "Type of the size definer." +
            "Possible types are: Frequency")]
        public SizeDefinersType SizeDefiner { get; set; }

        [Option("inf", Required = false, Default = false, HelpText = "Get words in infinitive form")]
        public bool Infinitive { get; set; }

        [Option('o', "outputFormat", Required = false, HelpText = "Format of image")]
        public ImageFormat Format { get; set; }

        public static Result<Options> Parse(IEnumerable<string> args)
        {
            var options = new Result<Options>();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => options = o)
                .WithNotParsed(e => options = Result.Fail<Options>("Wrong options"));
            return options;
        }

        private static Result<Options> CheckOptions(Options options)
        {
            var result = options.AsResult();
            foreach (var check in Checkups.Where(check => !check.Condition(options)))
                if (result.Error == null)
                    result = Result.Fail<Options>(check.Error);
                else
                    result.ReplaceError(e => $"{e} {check.Error}");

            return result;
        }

        private class Checker
        {
            public readonly Func<Options, bool> Condition;
            public readonly string Error;

            public Checker(Func<Options, bool> condition, string error)
            {
                Condition = condition;
                Error = error;
            }
        }
    }
}