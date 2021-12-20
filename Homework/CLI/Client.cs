using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudContainer;
using TagsCloudContainer.ClientsInterfaces;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;

namespace CLI
{
    public class Client : IClient
    {
        private readonly Dictionary<string, Func<string, string>> modifiers;
        private readonly string[] args;
        public IUserConfig UserConfig { get; }

        public Client(string[] args)
        {
            this.args = args;
            modifiers = new Dictionary<string, Func<string, string>>
            {
                {"lower", s => s.ToLower()},
                {"trim", s => s.Trim()}
            };
            UserConfig = ParseArguments().GetValueOrThrow();
        }

        private Result<CommandLineConfig> ParseArguments()
        {
            var config = new Result<CommandLineConfig>();
            var result = Parser.Default.ParseArguments<Options>(args);
            result.WithParsed(options => config = GetConfigResult(options))
                .WithNotParsed(errs => throw new Exception($"Failed with errors:\n{string.Join("\n", errs)}"));

            return config;
        }

        private Result<CommandLineConfig> GetConfigResult(Options options)
        {
            var config = new CommandLineConfig();
            return config.AsResult()
                .Then(UseOutputPathFrom, options)
                .Then(UseImageSizeFrom, options)
                .Then(BuildImageCenter)
                .Then(UseFontParamsFrom, options)
                .Then(UseColorSchemeFrom, options)
                .Then(UseSpiralFrom, options)
                .Then(UseImageFormatFrom, options)
                .Then(UseTextFormatFrom, options)
                .Then(UseSourceReaderFrom, options)
                .Then(UseHandlerConveyorFrom, options)
                .Then(BuildTextParser);
        }

        private Result<T> CheckUsedArg<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);
        }

        private Result<CommandLineConfig> UseOutputPathFrom(CommandLineConfig config,
            Options options)
        {
            config.OutputFilePath = options.Output;
            return CheckUsedArg(config, c => Directory.Exists(c.OutputFilePath), "Output directory doesn't exist!");
        }


        private Result<CommandLineConfig> UseImageSizeFrom(CommandLineConfig config,
            Options options)
        {
            config.ImageSize = new Size(options.Width, options.Height);
            return CheckUsedArg(config, c => c.ImageSize.Width > 0 && c.ImageSize.Height > 0,
                "Image size must be greater than zero!");
        }

        private Result<CommandLineConfig> BuildImageCenter(CommandLineConfig config)
        {
            var imageSize = config.ImageSize;
            config.ImageCenter = new Point(imageSize.Width / 2, imageSize.Height / 2);
            return config.AsResult();
        }

        private Result<CommandLineConfig> UseFontParamsFrom(CommandLineConfig config,
    Options options)
        {
            config.TagsFontName = options.FontName;
            config.TagsFontSize = options.FontSize;
            return CheckUsedArg(config, CheckFontParams, "Font name is unknown!");
        }

        private bool CheckFontParams(CommandLineConfig config)
        {
            using (var font = new Font(config.TagsFontName, config.TagsFontSize))
                return font.Name == config.TagsFontName;
        }

        private Result<CommandLineConfig> UseColorSchemeFrom(CommandLineConfig config,
            Options options)
        {
            config.TagsColors = GetColorScheme(options);
            return CheckUsedArg(config, c => c.TagsColors != null, "Unknown color scheme is given!");
        }

        private IColorScheme GetColorScheme(Options options)
        {
            switch (options.Color)
            {
                case 0: return new BlackWhiteScheme();
                case 1: return new CamouflageScheme();
                case 2: return new CyberpunkScheme();
                default: return null;
            }
        }

        private Result<CommandLineConfig> UseSpiralFrom(CommandLineConfig config,
           Options options)
        {
            config.Spiral = GetSpiral(options);
            return CheckUsedArg(config, c => c.Spiral != null, "Unknown color scheme is given!");
        }

        private ISpiral GetSpiral(Options options)
        {
            var spiralName = options.Spiral.ToLower();
            var imageCenter = new Point(options.Width / 2, options.Height / 2);
            switch (spiralName)
            {
                case "log": return new LogarithmSpiral(imageCenter);
                case "sqr": return new SquareSpiral(imageCenter);
                case "rnd": return new RandomSpiral(imageCenter);
                default: return null;
            }
        }

        private Result<CommandLineConfig> UseImageFormatFrom(CommandLineConfig config,
            Options options)
        {
            config.ImageFormat = GetImageFormat(options);
            return CheckUsedArg(config, c => c.ImageFormat != null, "Unknown color scheme is given!");
        }

        private ImageFormat GetImageFormat(Options options)
        {
            var formatName = options.OutputFileFormat.ToLower();
            switch (formatName)
            {
                case "png": return ImageFormat.Png;
                case "bmp": return ImageFormat.Bmp;
                case "jpeg": return ImageFormat.Jpeg;
                default: return null;
            }
        }

        private Result<CommandLineConfig> UseTextFormatFrom(CommandLineConfig config,
            Options options)
        {
            config.InputFileFormat = options.InputFileFormat;
            return CheckUsedArg(config, c => c.InputFileFormat == "txt", "Unknown input file format is given!");
        }

        private Result<CommandLineConfig> UseSourceReaderFrom(CommandLineConfig config,
           Options options)
        {
            config.SourceReader = GetSourceReader(options);
            return CheckUsedArg(config, c => c.SourceReader != null,
                "You need to set input file or words list!");
        }

        private ISourceReader GetSourceReader(Options options)
        {
            if (options.Input != null) return new TxtFileReader(options.Input);
            if (options.Tags != null) return new TagsReader(options.Tags);
            return null;
        }
    }
}
