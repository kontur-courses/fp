using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using CTV.Common.VisualizerContainer;
using CTV.ConsoleInterface.ConsoleCommands;
using CTV.ConsoleInterface.Options;

namespace CTV.ConsoleInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            args = new[]
            {
                "visualizeFromConfig",
            };
            ParseDefaultOptions(args);
        }

        private static void ParseDefaultOptions(string[] args)
        {
            Parser
                .Default
                .ParseArguments<VisualizeCommand, VisualizeFromConfigCommand, StartLoopCommand>(args)
                .WithParsed<VisualizeCommand>(command => VisualizeOnce(command.ToVisualizerOptions()))
                .WithParsed<VisualizeFromConfigCommand>(command => VisualizeOnce(command.ReadConfig()))
                .WithParsed<StartLoopCommand>(StartLoop);
        }
        
        private static void ParseLoopOptions(string[] args)
        {
            Parser
                .Default
                .ParseArguments<VisualizeCommand, VisualizeFromConfigCommand, ExitLoopCommand>(args)
                .WithParsed<VisualizeCommand>(command => VisualizeOnce(command.ToVisualizerOptions()))
                .WithParsed<VisualizeFromConfigCommand>(command => VisualizeOnce(command.ReadConfig()))
                .WithParsed<ExitLoopCommand>(command => command.ExitLoop());
        }

        private static void VisualizeOnce(VisualizerOptions options)
        {
            var consoleProcessor = new ConsoleProcessor();
            consoleProcessor.Run(options);
        }

        private static void StartLoop(StartLoopCommand commands)
        {
            Console.WriteLine("--help to see commands");
            while (true)
            {
                var args = Console
                    .ReadLine()?
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                ParseLoopOptions(args);
            }
        }
    }
}