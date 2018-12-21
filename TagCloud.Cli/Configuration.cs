using System;
using System.Drawing;
using System.IO;
using CommandLine;
using TagCloud.Enums;

namespace TagCloud
{
    public class Configuration
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string StopWordsFile { get; set; }

        private Color backgroundColor;
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (!value.IsKnownColor)
                    throw new ArgumentException("Unknown color for background: " + value.Name);
                backgroundColor = value;
            }
        }

        private Size imageSize;

        public Size ImageSize
        {
            get => imageSize;
            set
            {
                if (value.Height <= 0)
                    throw new ArgumentException("Result image height must be a greater then zero number, but given: " + value.Height);
                if (value.Width <= 0)
                    throw new ArgumentException("Result image width must be a greater then zero number, but given: " + value.Width);
                imageSize = value;
            }
        }

        public CloudLayouterType LayouterType { get; set; }
        public ColorScheme ColorScheme { get; set; }
        public FontScheme FontScheme { get; set; }
        public SizeScheme SizeScheme { get; set; }
        public bool IgnoreBoring { get; set; }

        public static Result<Configuration> FromArguments(string[] args)
        {
            return Result.Of(() => new Configuration())
                .Then(configuration =>
                {
                    Parser.Default.ParseArguments<Options>(args)
                        .WithParsed(o => configuration.InputFile = o.Input)
                        .WithParsed(o => configuration.OutputFile = o.Output)
                        .WithParsed(o => configuration.StopWordsFile = o.Stopwords)
                        .WithParsed(o => configuration.BackgroundColor = Color.FromName(o.Background))
                        .WithParsed(o => configuration.ImageSize = new Size(o.Width, o.Height))
                        .WithParsed(o => configuration.ColorScheme = o.ColorScheme)
                        .WithParsed(o => configuration.FontScheme = o.FontScheme)
                        .WithParsed(o => configuration.LayouterType = o.Layouter)
                        .WithParsed(o => configuration.SizeScheme = o.SizeScheme)
                        .WithParsed(o => configuration.IgnoreBoring = o.IgnoreBoring)
                        .WithNotParsed(o => throw new ArgumentException("Wrong command line arguments"));
                    return configuration;
                });
        }
    }
}