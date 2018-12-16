using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using CommandLine;
using jdk.@internal.org.objectweb.asm.util;
using TagsCloud.ErrorHandling;
using TagsCloud.TagsCloudVisualization.ColorSchemes;
using TagsCloud.TagsCloudVisualization.SizeDefiners;
using TagsCloud.WordPrework;

namespace TagsCloud
{
    public class Options
    {
        private class Check
        {
            public readonly Func<Options, bool> Predicate;
            public readonly string ErrorMessage;

            public Check(Func<Options, bool> predicate, string errorMessage)
            {
                Predicate = predicate;
                ErrorMessage = errorMessage;
            }
        }

        public static Result<Options> Parse(string[] args)
        {
            var result = new Result<Options>();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => result = CheckOptions(o))
                .WithNotParsed(e => result = Result.Fail<Options>(""));
            return result;
        }

        private static List<Check> checks = new List<Check>
        {
            new Check(o => o.DAngle>0, "Dangle should be greater than 0."),
            new Check(o => o.Width>0, "Width should be greater than 0."),
            new Check(o => o.Height>0, "Height should be greater than 0."),
            new Check(o => o.MinFontSize>0, "Minimum font size should be greater than 0."),
            new Check(o => o.MaxFontSize>0, "Maximum font size should be greater than 0."),
            new Check(o => o.MaxFontSize>=o.MinFontSize, "Maximum font size should be greater or equal to minimum font size."),
            new Check(o =>
            {
                var color = Color.FromName(o.BackgroundColor);
                return color.IsKnownColor;
            }, "Wrong background color name."),
            new Check(o =>
            {
                var font = new Font(o.Font, 10);
                return o.Font == font.Name;
            }, "Wrong background color name.")
        };

        private static Result<Options> CheckOptions(Options options)
        {
            var result = options.AsResult();
            foreach (var check in checks)
            {
                if (!check.Predicate(options))
                {
                    if (result.Error == null)
                        result = Result.Fail<Options>(check.ErrorMessage);
                    else
                        result.ReplaceError(e => $"{e} {check.ErrorMessage}");
                }
            }

            return result;
        }

        [Option('f', "file", Required = true, HelpText = "File with words to build tag cloud from.")]
        public string File{ get; set; }

        [Option('w', "width", Required = false, Default = 1024, HelpText = "Defines width of the picture.")]
        public int Width { get; set; }

        [Option('h', "height", Required = false, Default = 1024, HelpText = "Defines width of the picture.")]
        public int Height { get; set; }

        [Option("font", Required = false, Default = "Arial", HelpText = "Defines font of words.")]
        public string Font { get; set; }

        [Option("bgcolor", Required = false, Default = "LightGray", HelpText = "Defines color of the background.")]
        public String BackgroundColor { get; set; }

        [Option("colorScheme", Required = false, Default = ColorScheme.Red, HelpText = "Defines color scheme of the font.")]
        public ColorScheme ColorScheme { get; set; }


        [Option("boring", Required = false, Default = null, HelpText = "Defines boring parts of speech that will not be listed in result. " +
                                                           "Possible parts are: Adjective, Adverb, PronominalAdverb, NumeralAdjective, " +
                                                           "PronounAdjective, CompositePart, Conjunction, Interjection, Numeral, Particle, " +
                                                           "Pretext, Noun, PronounNoun, Verb")]
        public IEnumerable<PartOfSpeech> BoringParts { get; set; }


        [Option("only", Required = false, Default = null, HelpText = "Defines boring parts of speech that will not be listed in result. " +
                                                         "Possible parts are: Adjective, Adverb, PronominalAdverb, NumeralAdjective, " +
                                                         "PronounAdjective, CompositePart, Conjunction, Interjection, Numeral, Particle, " +
                                                         "Pretext, Noun, PronounNoun, Verb")]
        public IEnumerable<PartOfSpeech> PartsToUse { get; set; }

        [Option("infinitive", Required = false, Default = false, HelpText = "Uses infinitive form of words.")]
        public bool Infinitive { get; set; }

        [Option("dangle", Required = false, Default = Math.PI/16, HelpText = "Defines dangle in spiral cloud.")]
        public double DAngle { get; set; }

        [Option("minFontSize", Required = false, Default = 10, HelpText = "Defines minimum font size.")]
        public int MinFontSize { get; set; }

        [Option("maxFontSize", Required = false, Default = 100, HelpText = "Defines maximum font size.")]
        public int MaxFontSize { get; set; }

        [Option("sizeDefiner", Required = false, Default = SizeDefiner.Frequency, HelpText = "Defines type of the size definer." +
                                                                                             "Possible types are: Random, Frequency")]
        public SizeDefiner SizeDefinerType { get; set; }
    }
}
