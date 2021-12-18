using System;
using System.IO;
using System.Linq;
using Autofac;
using CommandLine;
using ResultMonad;
using TagsCloudVisualization.Module;

namespace TagsCloudVisualization.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (!result.Errors.Any())
            {
                Run(result.Value);
            }
        }

        private static void Run(Options options)
        {
            var directory = Path.GetFullPath(options.OutputDirectory ?? Options.DefaultOutputDirectory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filename = Path.Combine(directory, options.OutputFileName ?? GenerateName());
            var container = new ContainerBuilder()
                .RegisterTagsClouds(options.ToDrawerSettings())
                .RegisterImageCreation(filename)
                .Build();
            container.Resolve<TagsCloudVisualizer>()
                .Visualize(options.MaxTags)
                .OnSuccess(() => Console.WriteLine($"Tags cloud {filename} generated."))
                .OnFail(err => Console.WriteLine($"Error: {err}"));
        }

        private static string GenerateName() =>
            $"Cloud_{DateTime.Now:dd-MM-yy}_{DateTime.Now.Subtract(DateTime.Today).TotalSeconds:0}";
    }
}