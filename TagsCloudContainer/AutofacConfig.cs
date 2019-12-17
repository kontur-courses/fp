using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using Autofac;
using DocoptNet;
using FluentAssertions.Common;
using ResultOf;
using TagsCloudContainer.TagCloudVisualization;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer
{
    public static class AutofacConfig
    {
        public static Result<IContainer> ConfigureContainer(IDictionary<string, ValueObject> arguments)
        {
            return Result.Of(() => new ContainerBuilder())
                .Then(builder => builder.RegisterImageFormat(arguments["--format"].ToString()))
                .Then(builder => builder.RegisterSettings(ConfigureSettings(arguments)))
                .Then(builder => builder.RegisterWordProvider(arguments["<inputFile>"]?.ToString()))
                .Then(builder => builder.RegisterTypesAsInterfaces())
                .Then(builder => builder.Build())
                .RefineError("Failed configuring container");
        }

        private static Result<ContainerBuilder> RegisterImageFormat(this ContainerBuilder builder, string strImgFormat)
        {
            var imgFormatResult = ArgumentsParser.ParseImageFormat(strImgFormat);
            var imgFormat = imgFormatResult.GetValueOrThrow();
            builder.Register(c => imgFormat).As<ImageFormat>();
            return builder;
        }

        private static Result<ContainerBuilder> RegisterSettings(this ContainerBuilder builder,
                                                                 Result<VisualizationSettings> settingsResult)
        {
            return Result.Of(() =>
            {
                var settings = settingsResult.GetValueOrThrow();
                builder.Register(c => settings.Font).As<Font>();
                builder.Register(c => settings).As<VisualizationSettings>();
                return builder;
            });
        }

        private static Result<VisualizationSettings> ConfigureSettings(IDictionary<string, ValueObject> arguments)
        {
            var fontResult = ArgumentsParser.ParseFont(arguments["--font"].ToString(), 
                arguments["--fontSize"].AsInt);
            var backgroundBrushResult = ArgumentsParser.ParseBrush(arguments["--bgcolor"].ToString());
            var textBrushResult = ArgumentsParser.ParseBrush(arguments["--textcolor"].ToString());
            var isDebugMode = arguments["--debug"].IsTrue;
            var debugItemRectangleColorResult = ArgumentsParser.ParseColor(arguments["--dbgrectcolor"].ToString());
            var debugMarkingColorResult = ArgumentsParser.ParseColor(arguments["--dbgmarkcolor"].ToString());
            var imageSizeResult = ArgumentsParser.ParseSize(arguments["--size"].ToString());

            return Result.Of(() => VisualizationSettings.CreateDefaultSettings().GetValueOrThrow())
                .Then(settings => settings.WithFont(fontResult.GetValueOrThrow()))
                .Then(settings => settings.WithBackgroundBrush(backgroundBrushResult.GetValueOrThrow()))
                .Then(settings => settings.WithTextBrush(textBrushResult.GetValueOrThrow()))
                .Then(settings => settings.WithDebugMode(isDebugMode))
                .Then(settings => settings.WithDebugMarkingColor(debugMarkingColorResult.GetValueOrThrow()))
                .Then(settings => settings.WithDebugItemRectangleColor(debugItemRectangleColorResult.GetValueOrThrow()))
                .Then(settings => settings.WithSize(imageSizeResult.GetValueOrThrow()))
                .RefineError("Failed configuring settings");
        }

        private static Result<ContainerBuilder> RegisterWordProvider(this ContainerBuilder builder, string inputSource)
        {
            if (inputSource != null)
                return builder.RegisterFromFileReader(inputSource);
            
            builder.RegisterType<ConsoleReader>().As<IWordProvider>();
            return builder;
        }

        private static Result<ContainerBuilder> RegisterFromFileReader(this ContainerBuilder builder, string inputSource)
        {
            var inputFormat = ResolveInputFormat(inputSource);
            var wordProviderType = Assembly.GetExecutingAssembly()
                                           .GetTypes()
                                           .Where(type => typeof(IWordProvider).IsAssignableFrom(type))
                                           .FirstOrDefault(t => t.Name == $"{inputFormat.Capitalize()}Reader");
            if (wordProviderType is null)
                return Result.Fail<ContainerBuilder>($"Unsupported input file format: {inputFormat}");
            
            builder.RegisterType(wordProviderType).As<IWordProvider>()
                .WithParameter(new TypedParameter(typeof(string), inputSource));
            return builder;

        }

        private static string ResolveInputFormat(string inputSource)
        {
            var a = inputSource.Split(".");
            return a[a.Length - 1];
        }

        private static Result<ContainerBuilder> RegisterTypesAsInterfaces(this ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(type => !typeof(IWordProvider).IsAssignableFrom(type))
                .AsImplementedInterfaces();
            return builder;
        }
    }
}