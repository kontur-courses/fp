using CommandLine;
using Results;
using System;

namespace TagsCloudConsoleUI
{
    internal static class ConsoleManager
    {
        private static Parser InitCommandParser()
        {
            return new Parser(config =>
            {
                config.HelpWriter = Console.Out;
                config.EnableDashDash = true;
            });
        }

        public static void Run<T>(IConsoleManagerFormatter formatter, Func<ConsoleParsedOptions, Result<T>> onCallAction)
        {
            var commandParser = InitCommandParser();
            Console.WriteLine(formatter.InitialMessage);

            while (true)
            {
                Console.WriteLine('\n' + formatter.BorderString(Console.WindowWidth));
                var command = Console.ReadLine()?.Split(' ');
                Console.WriteLine();

                commandParser.ParseArguments<ConsoleParsedOptions>(command)
                    .WithParsed(options =>
                    {
                        var action = onCallAction(options);
                        if (action.IsSuccess)
                            Console.WriteLine(formatter.SuccessfulMessage(options.OutputFilePath));
                        else
                            Console.WriteLine(formatter.ErrorMessage + '\n' + action.Error);
                    })
                    .WithNotParsed(errors =>
                    {
                        Console.WriteLine(formatter.ParseCommandErrorMessage);
                        foreach (var error in errors)
                            Console.WriteLine(formatter.ErrorSymbol + error);
                    });
            }
        }
    }
}