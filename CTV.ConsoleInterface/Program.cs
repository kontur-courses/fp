using System;
using System.Collections.Generic;
using System.Linq;
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
                .OnFail(Console.Error.WriteLine);
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
                    errors => OnError(errors)
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
                    errors => OnError(errors)
                );
        }

        private static Result<None> VisualizeOnce(VisualizerOptions options)
        {
            return Result.Ok(new ConsoleProcessor())
                .Then(processor => processor.Run(options))
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
                    .OnFail(Console.WriteLine)
                    .OnSuccess(() => Console.WriteLine("Successfully visualized image"));
            }
        }

        private static Result<None> OnError(IEnumerable<Error> errors)
        {
            return errors.Any(x => x is not HelpRequestedError and not VersionRequestedError)
                ? Result.Fail<None>("Error in parsing line commands")
                : Result.Ok();

        }
    }
}