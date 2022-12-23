using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagCloud.CommandLineParsing;
using TagCloud.FileReader;
using TagCloud.ImageProcessing;
using TagCloud.ResultMonade;

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

            var formats = Enum.GetNames(typeof(ValidInputFileFormats));
            if (!formats.Any(t => options.InputFileFullPath.Contains(t)))
                return Result.Fail<Options>($"Input file {options.InputFileFullPath} has invalid format");

            if (!options.OutputImageFullPath.EndsWith(".png"))
                return Result.Fail<Options>($"Output file {options.OutputImageFullPath} has invalid format");

            var cloudForms = new string[] { "circle", "ellipse"};
            if (!cloudForms.Contains(options.CloudForm.ToLower()))
                return Result.Fail<Options>($"Cloud form {options.CloudForm} isn't supported");

            if (options.ImageWidth <= 0 || options.ImageHeight <= 0)
                return Result.Fail<Options>($"Image size {options.ImageWidth} x {options.ImageHeight} is invalid");
         
            if (!Color.FromName(options.BackgroundColor).IsKnownColor)
                return Result.Fail<Options>($"Background color {options.BackgroundColor} is invalid");

            if (!Result.Of(() => new FontFamily(options.FontFamily)).IsSuccess)
                return Result.Fail<Options>($"Font family {options.FontFamily} is invalid");

            if (options.MinFontSize <= 0)
                return Result.Fail<Options>($"Font min size {options.MinFontSize} is invalid");

            if (options.MaxFontSize <= 0)
                return Result.Fail<Options>($"Font max size {options.MaxFontSize} is invalid");

            if (options.MaxFontSize < options.MinFontSize)
                return Result.Fail<Options>($"Font sizes are invalid. Max size have to be more than min size");

            var colorings = new string[] { "random", "gradient", "black" };
            if (!colorings.Contains(options.WordColoring.ToLower()))
                return Result.Fail<Options>($"Word coloring {options.WordColoring} isn't supported");

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
