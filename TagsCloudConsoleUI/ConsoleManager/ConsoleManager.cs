using CommandLine;
using ResultPattern;
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

        public static void Run<T>(IConsoleManagerFormatter formatter, Func<BuildOptions, Result<T>> onCallAction)
        {
            var commandParser = InitCommandParser();
            Console.WriteLine(formatter.InitialMessage);

            while (true)
            {
                Console.WriteLine('\n' + formatter.BorderString(Console.WindowWidth));
                var command = Console.ReadLine()?.Split(' ');
                Console.WriteLine();

                commandParser.ParseArguments<BuildOptions>(command)
                    .WithParsed(options =>
                    {
                        var result = onCallAction(options);
                        if(result.IsSuccess)
                            Console.WriteLine(formatter.SuccessfulMessage(options.OutputFileName));
                        else
                            Console.WriteLine(formatter.ErrorMessage + '\n' + result.Error);
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