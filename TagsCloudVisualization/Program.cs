using System;
using System.Collections.Generic;
using CommandLine;
using TagsCloudVisualization.Commands;
using TagsCloudVisualization.Processors;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<CommandLineOptions, CreateCloudCommand, ShowDemoCommand>(args)
                .MapResult(
                    (CreateCloudCommand options) => new CreateCloudProcessor().Run(options),
                    (ShowDemoCommand options) => new ShowDemoProcessor().Run(options),
                    HandleParseError);
        }

        private static int HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                Console.WriteLine(error.ToString());

            return 1;
        }
    }
}