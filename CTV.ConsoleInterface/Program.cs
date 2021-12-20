using System;
using CommandLine;
using CTV.Common.VisualizerContainer;

namespace CTV.ConsoleInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            args = new[]
            {
                "visualize", "--textToVisualize", "Samples\\input.txt", "--pathToSaveImage", "result.png"
            };
            ParseDefaultOptions(args);
        }

        private static void ParseDefaultOptions(string[] args)
        {
            Parser
                .Default
                .ParseArguments<VisualizerOptions, ShowDemoOptions>(args)
                .WithParsed<VisualizerOptions>(VisualizeOnce)
                .WithParsed<ShowDemoOptions>(ShowDemo);
        }
        
        private static void ParseDemoOptions(string[] args)
        {
            Parser
                .Default
                .ParseArguments<VisualizerOptions, ExitOptions>(args)
                .WithParsed<VisualizerOptions>(VisualizeOnce)
                .WithParsed<ExitOptions>(ExitDemo);
        }

        private static void VisualizeOnce(VisualizerOptions options)
        {
            var consoleProcessor = new ConsoleProcessor();
            consoleProcessor.Run(options);
        }

        private static void ShowDemo(ShowDemoOptions options)
        {
            Console.WriteLine("--help to see commands");
            while (true)
            {
                var args = Console
                    .ReadLine()?
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                ParseDemoOptions(args);
            }
        }

        private static void ExitDemo(ExitOptions options)
        {
            Console.WriteLine("Exiting demo");
            Environment.Exit(0);
        }
    }
}