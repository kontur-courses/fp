using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Autofac;
using DocoptNet;
using TagCloudResult;
using TagsCloudTextProcessing.Filters;
using TagsCloudTextProcessing.Formatters;
using TagsCloudTextProcessing.Readers;
using TagsCloudTextProcessing.Shufflers;
using TagsCloudTextProcessing.Tokenizers;
using TagsCloudTextProcessing.WordsIntoTokensTranslators;
using TagsCloudVisualization.BitmapSavers;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Styling;
using TagsCloudVisualization.Styling.TagColorizer;
using TagsCloudVisualization.Styling.TagSizeCalculators;
using TagsCloudVisualization.Styling.Themes;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudConsole
{
    public static class ContainerConfigurator
    {
        public static Result<IContainer> Configure(IDictionary<string, ValueObject> parameters)
        {
            return ConfigureApplication(
                    parameters["--w"],
                    parameters["--h"],
                    parameters["--output"].ToString(),
                    new ContainerBuilder())
                .Then(b => ConfigureTextReader(
                    parameters["--input"].ToString(),
                    parameters["--input_ext"].ToString(),
                    b))
                .Then(b => ConfigureTokenizer(parameters["--split_pattern"].ToString(), b))
                .Then(ConfigureWordsFormatter)
                .Then(b => ConfigureWordsFilter(parameters["--exclude"].ToString(), b))
                .Then(ConfigureWordsIntoTokenTranslator)
                .Then(b => ConfigureShuffler(parameters["--shuffle"].ToString(), parameters["--seed"], b))
                .Then(b => ConfigureFontProperties(parameters["--font"].ToString(), parameters["--font_size"], b))
                .Then(b => ConfigureTheme(parameters["--theme"].ToString(), b))
                .Then(ConfigureTagSizeCalculator)
                .Then(b => ConfigureColorizer(parameters["--colorize"].ToString(), b))
                .Then(ConfigureVisualizer)
                .Then(b => ConfigureSpiral(
                    parameters["--x"].ToString(),
                    parameters["--y"].ToString(),
                    parameters["--rad"].ToString(),
                    parameters["--incr"].ToString(),
                    parameters["--angle"].ToString(),
                    b))
                .Then(ConfigureLayouter)
                .Then(b => ConfigureSaver(parameters["--output_ext"].ToString(), b))
                .Then(b => b.Build());
        }

        private static Result<ContainerBuilder> ConfigureApplication(ValueObject width, ValueObject height,
            string outputPath,
            ContainerBuilder builder)
        {
            var widthResult = ValidatePositiveInteger(width, "WIDTH");
            if (!widthResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(widthResult.Error);

            var heightResult = ValidatePositiveInteger(height, "HEIGHT");
            if (!heightResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(heightResult.Error);

            var applicationParameters = typeof(Application).GetConstructors()[0].GetParameters();
            builder
                .RegisterType<Application>()
                .WithParameter(applicationParameters[0].Name, widthResult.GetValueOrThrow())
                .WithParameter(applicationParameters[1].Name, heightResult.GetValueOrThrow())
                .WithParameter(applicationParameters[2].Name, outputPath);
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureTextReader(string path, string extension,
            ContainerBuilder builder)
        {
            Type readerType;
            switch (extension)
            {
                case "txt":
                    readerType = typeof(TxtTextReader);
                    break;
                case "docx":
                    readerType = typeof(DocxTextReader);
                    break;
                case "pdf":
                    readerType = typeof(PdfTextReader);
                    break;
                default:
                    return Result.Fail<ContainerBuilder>($"{extension} is incorrect input file extension");
            }

            var pathName = typeof(TxtTextReader).GetConstructors()[0].GetParameters()[0].Name;
            builder.RegisterType(readerType)
                .As<ITextReader>()
                .WithParameter(pathName, path);
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureTokenizer(string splitPattern, ContainerBuilder builder)
        {
            var patternName = typeof(Tokenizer).GetConstructors()[0].GetParameters()[0].Name;
            builder.RegisterType<Tokenizer>()
                .As<ITokenizer>()
                .WithParameter(patternName, splitPattern);
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureWordsFormatter(ContainerBuilder builder)
        {
            builder.RegisterType<FormatterLowercaseAndTrim>().As<IWordsFormatter>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureWordsFilter(string wordsToExcludePath,
            ContainerBuilder builder)
        {
            var wordsToExclude = new[] {""};
            if (wordsToExcludePath != "none")
            {
                if (!File.Exists(wordsToExcludePath))
                    return Result.Fail<ContainerBuilder>($"EXCLUDE_FILE {wordsToExcludePath} doesn't exist");
                wordsToExclude = File.ReadAllLines(wordsToExcludePath);
            }

            var excludeFromListFilterParameters = typeof(ExcludeFromListFilter).GetConstructors()[0].GetParameters();
            builder.RegisterType<ExcludeFromListFilter>()
                .As<IWordsFilter>()
                .WithParameter(excludeFromListFilterParameters[0].Name, wordsToExclude);
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureWordsIntoTokenTranslator(ContainerBuilder builder)
        {
            builder.RegisterType<WordsIntoTokenTranslator>().As<IWordsIntoTokenTranslator>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureShuffler(string shuffleType, ValueObject seed,
            ContainerBuilder builder)
        {
            switch (shuffleType)
            {
                case "a":
                    builder.RegisterType<AscendingCountShuffler>()
                        .As<ITokenShuffler>();
                    break;
                case "d":
                    builder.RegisterType<DescendingCountShuffler>()
                        .As<ITokenShuffler>();
                    break;
                case "r":
                    var randomSeedName = typeof(RandomShuffler).GetConstructors()[0].GetParameters()[0].Name;
                    var randomSeed = Environment.TickCount;
                    if (seed.IsInt)
                        randomSeed = seed.AsInt;
                    builder.RegisterType<RandomShuffler>()
                        .As<ITokenShuffler>()
                        .WithParameter(randomSeedName, randomSeed);
                    break;
                default:
                    return Result.Fail<ContainerBuilder>($"Unexpected SHUFFLE {shuffleType}");
            }

            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureFontProperties(string fontName, ValueObject fontSize,
            ContainerBuilder builder)
        {
            using (var font = new Font(fontName, 1))
            {
                if (font.Name != fontName)
                    return Result.Fail<ContainerBuilder>($"Can't find FONT {fontName}");
            }

            var sizeResult = ValidatePositiveInteger(fontSize, "FONT_SIZE");
            if (!sizeResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(sizeResult.Error);
            builder.RegisterInstance(new FontProperties(fontName, sizeResult.GetValueOrThrow())).SingleInstance();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureTheme(string themeName, ContainerBuilder builder)
        {
            Type themeType;
            switch (themeName)
            {
                case "p":
                    themeType = typeof(PixelArtTheme);
                    break;
                case "gr":
                    themeType = typeof(GrayDarkTheme);
                    break;
                case "go":
                    themeType = typeof(GodotEngineTheme);
                    break;
                default:
                    return Result.Fail<ContainerBuilder>($"THEME {themeName} is incorrect");
            }

            builder.RegisterType(themeType)
                .As<ITheme>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureTagSizeCalculator(ContainerBuilder builder)
        {
            builder.RegisterType<LogarithmicTagSizeCalculator>().As<TagSizeCalculator>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureColorizer(string colorizeMethod, ContainerBuilder builder)
        {
            Type colorizerType;
            switch (colorizeMethod)
            {
                case "r":
                    colorizerType = typeof(RandomTagColorizer);
                    break;
                case "s":
                    colorizerType = typeof(BySizeTagColorizer);
                    break;
                default:
                    return Result.Fail<ContainerBuilder>($"Unexpected colorizing METHOD {colorizeMethod}");
            }

            builder.RegisterType(colorizerType)
                .As<ITagColorizer>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureVisualizer(ContainerBuilder builder)
        {
            builder.RegisterType<TextNoRectanglesVisualizer>().As<ICloudVisualizer>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureSpiral(string x, string y, string radius, string increment,
            string angle, ContainerBuilder builder)
        {
            var xResult = ValidateFloat(x, "X");
            if (!xResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(xResult.Error);

            var yResult = ValidateFloat(y, "Y");
            if (!yResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(yResult.Error);

            var radiusResult = ValidateFloat(radius, "RADIUS");
            if (!radiusResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(radiusResult.Error);

            var incrementResult = ValidateFloat(increment, "INCREMENT");
            if (!incrementResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(incrementResult.Error);

            var angleResult = ValidateFloat(angle, "ANGLE");
            if (!angleResult.IsSuccess)
                return Result.Fail<ContainerBuilder>(angleResult.Error);
            builder
                .RegisterInstance(
                    new Spiral(
                        new PointF(xResult.GetValueOrThrow(), yResult.GetValueOrThrow()),
                        radiusResult.GetValueOrThrow(),
                        incrementResult.GetValueOrThrow(),
                        angleResult.GetValueOrThrow()))
                .SingleInstance();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureLayouter(ContainerBuilder builder)
        {
            builder.RegisterType<SpiralLayouter>().As<ICloudLayouter>();
            return Result.Ok(builder);
        }

        private static Result<ContainerBuilder> ConfigureSaver(string extension, ContainerBuilder builder)
        {
            Type saverType;
            switch (extension)
            {
                case "png":
                    saverType = typeof(PngBitmapSaver);
                    break;
                case "wmf":
                    saverType = typeof(WmfBitmapSaver);
                    break;
                case "jpeg":
                    saverType = typeof(JpegBitmapSaver);
                    break;
                default:
                    return Result.Fail<ContainerBuilder>($"IMAGE_EXT {extension} is incorrect");
            }

            builder.RegisterType(saverType)
                .As<IBitmapSaver>();
            return Result.Ok(builder);
        }

        private static Result<int> ValidatePositiveInteger(ValueObject parameter, string parameterName)
        {
            int value;
            if (parameter.IsInt)
            {
                value = parameter.AsInt;
                if (value <= 0)
                    return Result.Fail<int>($"{parameterName} should be positive");
            }
            else
                return Result.Fail<int>($"{parameterName} should be integer value");

            return Result.Ok(value);
        }

        private static Result<float> ValidateFloat(string parameter, string parameterName)
        {
            return float.TryParse(parameter, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? Result.Ok(result)
                : Result.Fail<float>($"{parameterName} should be float value with '.'");
        }
    }
}