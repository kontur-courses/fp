﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using Fclp;
using TagsCloudContainer.WordsReaders;

namespace TagsCloudContainer.Cmd
{
    public class ParserArgs
    {
        public string InputFilename { get; set; }

        public string OutputFilename { get; set; } = "result.png";

        public string ExcludeFilename { get; set; }

        public string FontFamily { get; set; } = "Arial";

        public double FontSize { get; set; } = 12;

        public double SpiralAngleStep { get; set; } = 10;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var parser = new FluentCommandLineParser<ParserArgs>();

            var cmdArgs = ConfigureParser(parser);

            var containerBuilder = new CloudContainerBuilder();

            var tagsCloudContainer = containerBuilder.BuildTagsCloudContainer();
            var reader = tagsCloudContainer
                .Resolve<IWordsReader>();

            var words = Enumerable.Empty<string>();

            using (var scope = tagsCloudContainer.BeginLifetimeScope())
            {
                Result.Of(() => parser.Parse(args))
                    .Then(p => HandleParserBehavour(p, p.HelpCalled, string.Empty))
                    .Then(p => HandleParserBehavour(p, p.HasErrors, p.ErrorText))
                    .Then(p => reader.GetWords(parser.Object.InputFilename))
                    .Then(rawWords => words = rawWords)
                    .Then(config => ReadCustomBoringWords(reader, parser.Object.ExcludeFilename))
                    .Then(excluded =>
                    {
                        var config = scope.Resolve<Config>()
                            .CreateConfig(cmdArgs.ImageSize, cmdArgs.Color);
                        config.CustomBoringWords = excluded;

                        return config;
                    })
                    .Then(config =>
                    {
                        config.Font = new Font(new FontFamily(parser.Object.FontFamily),
                            (float)parser.Object.FontSize);

                        return config;
                    })
                    .Then(config => scope.Resolve<TagsCloudBuilder>()
                        .Visualize(words, config))
                    .Then(image => image.Save(parser.Object.OutputFilename, ImageFormat.Png))
                    .OnFail(Console.WriteLine);
            }
        }

        private static Result<ICommandLineParserResult> HandleParserBehavour(
            ICommandLineParserResult parserResult,
            bool isFailed,
            string errorMessage)
        {
            return isFailed
                ? Result.Fail<ICommandLineParserResult>(errorMessage)
                : Result.Ok(parserResult);
        }

        private static Result<IEnumerable<string>> ReadCustomBoringWords(IWordsReader reader, string filename)
        {
            if (filename == null)
            {
                return Result.Ok(Enumerable.Empty<string>());
            }

            return reader
                .GetWords(filename);
        }

        private static CmdArguments ConfigureParser(FluentCommandLineParser<ParserArgs> parser)
        {
            var callbacks = new CmdCallbacks();

            parser.Setup(arg => arg.InputFilename)
                .As("input")
                .Required();

            parser.Setup(arg => arg.OutputFilename)
                .As("output");

            parser.Setup(arg => arg.ExcludeFilename)
                .As("exclude");

            parser.Setup(arg => arg.FontFamily)
                .As("font");

            parser.Setup(arg => arg.FontSize)
                .As("fontSize");

            parser.Setup(arg => arg.SpiralAngleStep)
                .As("spiralAngleStep");

            parser.Parser.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(callbacks.GetHelpInformation()));

            parser.Parser.Setup<string>("imageSize")
                .Callback(callbacks.SetImageSize);

            parser.Parser.Setup<string>("color")
                .Callback(callbacks.SetColor);

            parser.Parser.Setup<string>("spiralOffset")
                .Callback(callbacks.SetSpiralOffset);

            return callbacks.CmdArgs;
        }
    }
}