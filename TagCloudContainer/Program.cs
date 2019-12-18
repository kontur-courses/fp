using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using Fclp;
using Fclp.Internals.Parsing;
using TagCloudContainer.Api;
using TagCloudContainer.fluent;
using TagCloudContainer.Implementations;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer
{
    internal class Program
    {
        public static Dictionary<string, ImageFormat> imageFormats = new Dictionary<string, ImageFormat>
        {
            {"bmp", ImageFormat.Bmp}, {"emf", ImageFormat.Emf}, {"exif", ImageFormat.Exif},
            {"gif", ImageFormat.Gif}, {"icon", ImageFormat.Icon}, {"jpeg", ImageFormat.Jpeg},
            {"png", ImageFormat.Png}, {"tiff", ImageFormat.Tiff}, {"wmf", ImageFormat.Wmf},
            {"membmp", ImageFormat.MemoryBmp}
        };

        private static void Main(string[] args)
        {
            var cli = new Cli();
            var p = SetupParser(cli);
            var config = new Result<TagCloudConfig>(value: new TagCloudConfig(cli));

            var parserResult = p.Parse(args);

            var options = parserResult.RawResult.ParsedOptions.ToList();

            var inputFile = options.Where(o => o.Key == "f").Select(o => o.Value).Last();
            var outputFile = options.Where(o => o.Key == "o").Select(o => o.Value).Last();
            var parameters = options.Where(o => o.Key != "o" && o.Key != "f");

            config = parameters.Aggregate(config, (current, o) => ProcessOption(o, current, cli));

            var result = config.Then(cfg => cfg.CreateCloud(inputFile, outputFile));
            result.GetValueOrThrow();
        }

        private static Result<TagCloudConfig> ProcessOption(ParsedOption o, Result<TagCloudConfig> config, Cli cli)
        {
            var value = o.Value;
            config = o.Key switch
            {
                "source-type" => config.Then(cfg => cfg.UsingWordProvider(value, cli)),
                "format" => config.Then(cfg => cfg.SetImageFormat(imageFormats[value])),
                "size" => config.Then(cfg => cfg.SetSize(value)),
                "wordProcessor" => config.Then(cfg => cfg.UsingWordProcessor(value, cli)),
                "layout" => config.Then(cfg => cfg.UsingCloudLayouter(value, cli)),
                "wordLayouter" => config.Then(cfg => cfg.UsingWordCloudLayouter(value, cli)),
                "sizeProvider" => config.Then(cfg => cfg.UsingStringSizeProvider(value, cli)),
                "brushProvider" => config.Then(cfg => cfg.UsingWordBrushProvider(value, cli)),
                "penProvider" => config.Then(cfg => cfg.UsingRectanglePenProvider(value, cli)),
                "wordVisualizer" => config.Then(cfg => cfg.UsingWordVisualizer(value, cli)),
                _ => config
            };

            return config;
        }

        private static FluentCommandLineParser SetupParser(Cli cli)
        {
            var parser = new FluentCommandLineParser();
            parser.Setup<string>('f').Required();
            parser.Setup<string>('o').Required();

            parser.Setup<string>('p').Callback(useParameters =>
            {
                if (useParameters is null || useParameters.ToLower().Equals("true"))
                    cli.CollectCliElements();
            });
            parser.Setup<string>("source-type");
            parser.Setup<string>("format");
            parser.Setup<string>("size");
            parser.Setup<string>("wordProcessor");
            parser.Setup<string>("layout");
            parser.Setup<string>("wordLayouter");
            parser.Setup<string>("sizeProvider");
            parser.Setup<string>("brushProvider");
            parser.Setup<string>("penProvider");
            parser.Setup<string>("wordVisualizer");

            return parser;
        }
    }
}