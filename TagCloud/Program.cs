using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Fclp;
using TagCloud.Data;
using TagCloud.Data.ContainerModules;
using TagCloud.Drawer;
using TagCloud.Processor;
using TagCloud.Reader;
using TagCloud.Saver;
using TagCloud.WordsLayouter;

namespace TagCloud
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GetArguments(args)
                .Then(Run)
                .OnFail(Console.WriteLine);
        }

        private static Result<None> Run(Arguments arguments)
        {
            return GetContainer(arguments)
                .Then(container => container
                    .Build()
                    .Resolve<TagCloudGenerator>()
                    .Generate(arguments));
        }

        private static Result<Arguments> GetArguments(string[] args)
        {
            var arguments = new Arguments();
            var help = "";

            var parser = new FluentCommandLineParser();
            parser
                .SetupHelp("h", "?", "help")
                .Callback(str => help = str)
                .WithHeader("Program to create tag cloud. Options:");
            parser
                .Setup<string>('w')
                .Callback(file => arguments.WordsFileName = file)
                .WithDescription("Name of file with words");
            parser
                .Setup<string>('b')
                .Callback(boring => arguments.BoringWordsFileName = boring)
                .WithDescription("Name of file with boring words");
            parser
                .Setup<string>('i')
                .Callback(name => arguments.ImageFileName = name)
                .WithDescription("Name of result image (png, jpg, bmp)");
            parser
                .Setup<string>('c')
                .Callback(color => arguments.WordsColorName = color)
                .WithDescription("Words color");
            parser
                .Setup<string>('g')
                .Callback(color => arguments.BackgroundColorName = color)
                .WithDescription("Background color");
            parser
                .Setup<string>('f')
                .Callback(font => arguments.FontFamilyName = font)
                .WithDescription("Words font family");
            parser
                .Setup<int>('m')
                .Callback(size => arguments.Multiplier = size)
                .WithDescription("Words font size multiplier");
            parser
                .Setup<bool>('s')
                .Callback(save => arguments.ToEnableClipboardSaver = true)
                .WithDescription("Save image to clipboard");

            var result = parser.Parse(args);

            return result.HelpCalled 
                ? Result.Fail<Arguments>(help) 
                : CheckArguments(arguments);
        }

        private static Result<Arguments> CheckArguments(Arguments arguments)
        {
            var format = TextFileReader.GetFormat(arguments.ImageFileName);
            return CheckFile(arguments, "Words", arguments.WordsFileName)
                .Then(args => CheckFile(args, "Boring words", arguments.BoringWordsFileName))
                .Then(args => CheckArgument(args, FileImageSaver.Formats.Keys, "image format", format))
                .Then(args => CheckArgument(args, CloudDrawer.Colors, "words color", arguments.WordsColorName))
                .Then(args => CheckArgument(args, CloudDrawer.Colors, "background color", arguments.BackgroundColorName))
                .Then(args => CheckArgument(args, CloudWordsLayouter.Fonts, "font", arguments.FontFamilyName))
                .Then(CheckMultiplier);
        }

        private static Result<Arguments> CheckMultiplier(Arguments arguments)
        {
            return arguments.Multiplier > 0
                ? Result.Ok(arguments)
                : Result.Fail<Arguments>("Font size multiplier should be positive");
        }

        private static Result<Arguments> CheckFile(Arguments arguments, string argumentName, string fileName)
        {
            return File.Exists(fileName)
                ? Result.Ok(arguments)
                : Result.Fail<Arguments>($"{argumentName} file not found {fileName}");
        }

        private static Result<Arguments> CheckArgument<T>(Arguments arguments, ICollection<T> variants, string argumentName, T argument)
        {
            return variants.Contains(argument)
                ? Result.Ok(arguments)
                : Result.Fail<Arguments>($"Unknown {argumentName} {argument}");
        }

        public static Result<ContainerBuilder> GetContainer(Arguments arguments)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ConfigurationModule>();

            return GetWords(arguments)
                .Then(words => RegisterProcessors(builder, words));
        }

        private static Result<IEnumerable<string>> GetWords(Arguments arguments)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ReadersModule>();

            return builder
                .Build()
                .Resolve<IWordsFileReader>()
                .ReadWords(arguments.BoringWordsFileName);
        }

        private static Result<ContainerBuilder> RegisterProcessors(ContainerBuilder builder, IEnumerable<string> boringWords)
        {
            builder.RegisterType<WordsToLowerProcessor>().As<IWordsProcessor>();
            builder.Register(c => new BoringWordsProcessor(boringWords)).As<IWordsProcessor>();
            builder.RegisterType<StemWordsProcessor>().As<IWordsProcessor>();
            return builder;
        }
    }
}