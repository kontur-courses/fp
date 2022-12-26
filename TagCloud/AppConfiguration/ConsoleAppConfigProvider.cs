using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagCloud.CloudLayouter;
using TagCloud.CommandLineParsing;
using TagCloud.FileReader;
using TagCloud.ImageProcessing;
using TagCloud.ResultMonade;
using TagCloud.WordColoring;

namespace TagCloud.AppConfiguration
{
    public class ConsoleAppConfigProvider : IAppConfigProvider
    {
        public Result<IAppConfig> GetAppConfig(IEnumerable<string> args)
        {
            return Result.Of(() => Parser.Default.ParseArguments<Options>(args).Value)
                       .RefineError("Command line arguments can't be parsed")
                       .Then(ValidateOptions)
                       .Then(CreateAppConfig)
                       .RefineError("Application configuration can't be obtained");
        }

        private Result<Options> ValidateOptions(Options options)
        {
            if (!File.Exists(options.InputFileFullPath))
                return Result.Fail<Options>($"Input file {options.InputFileFullPath} doesn't exist");

            var inputFileFormats = Enum.GetNames(typeof(InputFileFormats));
            if (!inputFileFormats.Any(f => options.InputFileFullPath.Contains(f)))
                return Result.Fail<Options>($"Input file {options.InputFileFullPath} has invalid format. " +
                    $"Supported file formats are: {string.Join(", ", inputFileFormats)}");

            var outputFileFormats = Enum.GetNames(typeof(OutputFileFormats));
            if (!options.OutputImageFullPath.EndsWith(".png"))
                return Result.Fail<Options>($"Output file {options.OutputImageFullPath} has invalid format. " +
                    $"Supported file formats are: {string.Join(", ", outputFileFormats)}");

            var cloudForms = Enum.GetNames(typeof(CloudForms));
            if (!cloudForms.Contains(options.CloudForm.ToLower()))
                return Result.Fail<Options>($"Cloud form {options.CloudForm} is invalid. " +
                    $"Supported forms are: {string.Join(", ", cloudForms)}");

            if (options.ImageWidth <= 0 || options.ImageHeight <= 0)
                return Result.Fail<Options>($"Image size {options.ImageWidth} x {options.ImageHeight} is invalid. " +
                    $"Width and height must be more than zero");
         
            if (!Color.FromName(options.BackgroundColor).IsKnownColor)
                return Result.Fail<Options>($"Background color {options.BackgroundColor} isn't supported");

            if (!Result.Of(() => new FontFamily(options.FontFamily)).IsSuccess)
                return Result.Fail<Options>($"Font family {options.FontFamily} isn't supported");

            if (options.MinFontSize <= 0)
                return Result.Fail<Options>($"Font min size {options.MinFontSize} must be more than zero");

            if (options.MaxFontSize <= 0)
                return Result.Fail<Options>($"Font max size {options.MaxFontSize} must be more than zero");

            if (options.MaxFontSize < options.MinFontSize)
                return Result.Fail<Options>($"Font sizes are invalid. Max size must to be equal or greater than min size");
           
            var wordColorings = Enum.GetNames(typeof(WordColorings));
            if (!wordColorings.Contains(options.WordColoring.ToLower()))
                return Result.Fail<Options>($"Word coloring {options.WordColoring} isn't supported. " +
                    $"Supported colorings are: {string.Join(", ", wordColorings)}");

            return options;
        }   

        private Result<IAppConfig> CreateAppConfig(Options options)
        {
            var imageSettings = new ImageSettings()
            {
                Size = new Size(options.ImageWidth, options.ImageHeight),
                BackgroundColor = System.Drawing.Color.FromName(options.BackgroundColor),
                FontFamily = new System.Drawing.FontFamily(options.FontFamily),
                MinFontSize = options.MinFontSize,
                MaxFontSize = options.MaxFontSize,
                WordColoringAlgorithmName = options.WordColoring
            };

            return new AppConfig(options.InputFileFullPath,
                                 options.OutputImageFullPath,
                                 options.CloudForm,
                                 new Point(options.CentralPointX, options.CentralPointY),
                                 imageSettings);
        }
    }
}
