using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using FunctionalTools;
using TagsCloudGenerator.FileReaders;
using TagsCloudGenerator.Saver;
using TagsCloudGenerator.Tools;
using TagsCloudGenerator.Visualizer;

namespace TagsCloudGenerator.Client.Console
{
    public class ConsoleClient : IClient
    {
        public void Run()
        {
            Parser
                .Default
                .ParseArguments<Options>(Environment.GetCommandLineArgs())
                .WithParsed(Run)
                .WithNotParsed(HandleErrors);
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            System.Console.WriteLine("During argument parsing errors was occured:");

            foreach (var error in errors)
                System.Console.WriteLine(error);
        }

        private static void Run(Options options)
        {
            ConsoleDependenciesBuilder
                .BuildContainer(options)
                .Then(container => Execute(container, options))
                .OnFail(error => System.Console.WriteLine($"Couldn't create tag cloud: {error}"));
        }

        private static Result<None> Execute(ILifetimeScope container, Options options)
        {
            using (container)
            {
                var generator = container.Resolve<ICloudGenerator>();
                var readerFactory = container.Resolve<IReaderFactory>();
                var saver = container.Resolve<IImageSaver>();
                var visualizer = container.Resolve<ICloudVisualizer>();

                return PathHelper.GetFileExtension(options.InputPath)
                    .Then(readerFactory.GetReaderFor)
                    .Then(reader => reader.ReadWords(options.InputPath))
                    .Then(generator.Generate)
                    .Then(visualizer.Draw)
                    .Then(saver.Save);
            }
        }
    }
}