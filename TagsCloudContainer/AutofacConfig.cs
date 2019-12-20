using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                .Then(builder => ConfigureSettings(arguments).Then(builder.RegisterSettings))
                .Then(builder => builder.RegisterWordProvider(arguments["<inputFile>"]?.ToString()))
                .Then(RegisterTypesAsInterfaces)
                .Then(builder => builder.Build())
                .RefineError("Failed configuring container");
        }

        private static ContainerBuilder RegisterImageFormat(this ContainerBuilder builder, string strImgFormat)
        {
            var imgFormatResult = ArgumentsParser.ParseImageFormat(strImgFormat);
            imgFormatResult.Then(imgFormat => builder.Register(c => imgFormat).As<ImageFormat>());
            return builder;
        }

        private static ContainerBuilder RegisterSettings(this ContainerBuilder builder,
            VisualizationSettings settings)
        {
            builder.Register(c => settings.Font).As<Font>();
            builder.Register(c => settings).As<VisualizationSettings>();
            return builder;
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

            return VisualizationSettings.CreateDefaultSettings()
                .Then(settings => fontResult.Then(settings.WithFont))
                .Then(settings => backgroundBrushResult.Then(settings.WithBackgroundBrush))
                .Then(settings => textBrushResult.Then(settings.WithTextBrush))
                .Then(settings => settings.WithDebugMode(isDebugMode))
                .Then(settings => debugMarkingColorResult.Then(settings.WithDebugMarkingColor))
                .Then(settings => debugItemRectangleColorResult.Then(settings.WithDebugItemRectangleColor))
                .Then(settings => imageSizeResult.Then(settings.WithSize))
                .RefineError("Failed configuring settings");
        }

        private static ContainerBuilder RegisterWordProvider(this ContainerBuilder builder, string inputSource)
        {
            if (inputSource != null)
                return builder.RegisterFileReader(inputSource);

            builder.RegisterType<ConsoleReader>().As<IWordProvider>();
            return builder;
        }

        private static ContainerBuilder RegisterFileReader(this ContainerBuilder builder,
            string inputSource)
        {
            var inputFormat = Path.GetExtension(inputSource).Substring(1);
            var wordProviderType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IWordProvider).IsAssignableFrom(type))
                .FirstOrDefault(t => t.Name == $"{inputFormat.Capitalize()}Reader");
            if (wordProviderType is null)
                throw new ArgumentException($"Unsupported input file format: {inputFormat}");

            builder.RegisterType(wordProviderType).As<IWordProvider>()
                .WithParameter(new TypedParameter(typeof(string), inputSource));
            return builder;
        }

        private static ContainerBuilder RegisterTypesAsInterfaces(this ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(type => !typeof(IWordProvider).IsAssignableFrom(type))
                .AsImplementedInterfaces();
            return builder;
        }
    }
}