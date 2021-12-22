using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using TagCloud.Processor;

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
                .Resolve<ITagCloudProcessor>()
                .Run();
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
