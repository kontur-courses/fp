using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using TagsCloudVisualization.Commands;
using TagsCloudVisualization.Processors;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var container = ContainerConfig.ConfigureContainer().GetValueOrThrow();
            
            return Parser.Default.ParseArguments<CreateCloudCommand, ShowDemoCommand>(args)
                .MapResult(
                    (CreateCloudCommand options) => container.Resolve<CreateCloudProcessor>().Run(options),
                    (ShowDemoCommand options) => container.Resolve<ShowDemoProcessor>().Run(options),
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