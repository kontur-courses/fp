using System;
using Autofac;
using CommandLine;
using TagsCloudVisualization;

namespace TagsCloudVisualizationConsole
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();

            if (!Parser.Default.ParseArguments(args, options))
                return;

            var reporter = new ExceptionReporter(Console.WriteLine);
            TagsCloudVisualizationContainerConfig.GetContainer().Resolve<TagsCloudApp>().Run(options, reporter);
        }
    }
}
