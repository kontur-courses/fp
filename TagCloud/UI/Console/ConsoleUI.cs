using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.Readers;
using TagCloud.ResultMonad;
using TagCloud.Visualizers;
using TagCloud.Writers;

namespace TagCloud.UI.Console
{
    public class ConsoleUI : IUserInterface
    {
        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        private void Run(Options options)
        {
            DependencyConfigurator
                .GetConfiguredContainer(options)
                .AsResult()
                .Then(container => Execute(container, options))
                .OnFail(System.Console.WriteLine);
        }

        private void Execute(IContainer container, Options options)
        {
            var readerFactory = container.Resolve<IFileReaderFactory>();
            var boringWordsFilter = container.Resolve<BoringWordsFilter>();
            var textAnalyzer = container.Resolve<ITextAnalyzer>();
            var tagCreator = container.Resolve<ITagCreator>();
            var layouter = container.Resolve<ICloudLayouter>();
            var visualizer = container.Resolve<IVisualizer>();
            var tagColoringFactory = container.Resolve<ITagColoringFactory>();
            var writer = container.Resolve<IFileWriter>();

            readerFactory.Create(GetExtensionsFromFileName(options.ExcludedWordsFile))
                .Then(reader => reader.ReadFile(options.ExcludedWordsFile))
                .Then(words => boringWordsFilter.AddWords(words))
                .Then(n => readerFactory.Create(GetExtensionsFromFileName(options.InputFilename)))
                .Then(reader => reader.ReadFile(options.InputFilename)) 
                .Then(words => textAnalyzer.Analyze(words)) 
                .Then(wordFrequencies => tagCreator.Create(wordFrequencies)) 
                .Then(tags => layouter.PutTags(tags)) 
                .Then(tags => visualizer.DrawCloud(tags, tagColoringFactory)) 
                .Then(bitmap => writer.Write(bitmap, 
                    options.OutputFilename, 
                    GetExtensionsFromFileName(options.OutputFilename)))
                .OnFail(System.Console.WriteLine);
        }

        private static string GetExtensionsFromFileName(string filename)
        {
            var lastDotIndex = filename.LastIndexOf('.');
            return filename[(lastDotIndex + 1)..];
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                if (error.StopsProcessing)
                    throw new ArgumentException($"Wrong command-line arguments. {error}");
            }
        }
    }
}
