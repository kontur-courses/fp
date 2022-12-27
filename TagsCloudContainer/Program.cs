using System;
using System.IO;
using System.Linq;
using Autofac;
using CommandLine;
using TagsCloudContainer.App;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer
{
    public static class Program
    {
        public static string ProjectPath =>
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private static void Main(string[] args)
        {
            var options = Result.Of(
                () => Parser.Default.ParseArguments<Options>(args).Value,
                "Command line arguments cannot be parsed")
                .Then(ValidateOptions)
                .GetValueOrThrow();
            var app = CreateApp(options);
            app.Run();
        }

        private static IApp CreateApp(Options options)
        {
            var builder = new ContainerBuilder();
            var container = builder.ConfigureTagCloud(options);
            return container.Resolve<IApp>();
        }

        private static Result<Options> ValidateOptions(Options options)
        {
            if (options.Width < 0 || options.Height < 0)
                return Result.Fail<Options>("Incorrect image size.");

            if (options.CenterX < 0 || options.CenterX > options.Width ||
                options.CenterY < 0 || options.CenterY > options.Height)
                return Result.Fail<Options>("Center of the tag cloud should be on the image.");

            if (options.Count < 0)
                return Result.Fail<Options>("Count of words in cloud should be non-negative.");

            var inputFullPath = Path.Combine(ProjectPath, options.InputFile);
            if (!File.Exists(inputFullPath))
                return Result.Fail<Options>($"File {options.InputFile} not found.");

            var supportedOutputFormats = new[] { "png", "jpg", "jpeg" };
            if (!supportedOutputFormats.Any(f => options.OutputFile.EndsWith($".{f}")))
                return Result.Fail<Options>(
                    "Unsupported output file format. Supported formats: png, jpg, jpeg");

            if (options.MaxFontSize < options.MinFontSize)
                return Result.Fail<Options>("Max font size must be greater than the min size");

            return Result.Ok(options);
        }
    }
}