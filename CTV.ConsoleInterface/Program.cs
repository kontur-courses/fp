using System;
using CommandLine;
using CTV.ConsoleInterface.ConsoleCommands;
using CTV.ConsoleInterface.Options;
using FunctionalProgrammingInfrastructure;

namespace CTV.ConsoleInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Result
                .OfAction(() => ParseDefaultOptions(args))
                .RefineError("Failed")
                .OnFail(error => Console.Error.WriteLine(error));
        }

        private static Result<None> ParseDefaultOptions(string[] args)
        {
            return Parser
                .Default
                .ParseArguments<VisualizeCommand, VisualizeFromConfigCommand, StartLoopCommand>(args)
                .MapResult(
                    (VisualizeCommand command) => OnVisualizeCommand(command),
                    (VisualizeFromConfigCommand command) => OnVisualizeFromConfigCommand(command),
                    (StartLoopCommand command) => Result.OfAction(() => StartLoop(command)),
                    _ => Result.Fail<None>("Contained error in arguments")
                );
        }

        private static Result<None> ParseLoopOptions(string[] args)
        {
            return Parser
                .Default
                .ParseArguments<VisualizeCommand, VisualizeFromConfigCommand, ExitLoopCommand>(args)
                .MapResult(
                    (VisualizeCommand command) => OnVisualizeCommand(command),
                    (VisualizeFromConfigCommand command) => OnVisualizeFromConfigCommand(command),
                    (ExitLoopCommand command) => Result.OfAction(ExitLoopCommand.ExitLoop),
                    _ => Result.Fail<None>("Contained error in arguments")
                );
        }

        private static Result<None> VisualizeOnce(ConsoleProcessorOptions options)
        {
            return ConsoleProcessor.Render(options)
                .RefineError("Visualization failed");
        }

        private static Result<None> OnVisualizeCommand(VisualizeCommand command)
        {
            return Result
                .Of(command.ToVisualizerOptions)
                .Then(VisualizeOnce);
        }

        private static Result<None> OnVisualizeFromConfigCommand(VisualizeFromConfigCommand command)
        {
            return command
                .ReadConfig()
                .Then(VisualizeOnce);
        }

        private static void StartLoop(StartLoopCommand commands)
        {
            Console.WriteLine("--help to see commands");
            while (true)
            {
                var args = Console
                    .ReadLine()?
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                ParseLoopOptions(args)
                    .OnFail(e => Console.Error.WriteLine(e))
                    .OnSuccess(() => Console.WriteLine("Successfully visualized image"));
            }
        }
    }
}