using System;
using System.Collections.Generic;
using CommandLine;
using TagsCloud.Words.Options;

namespace TagsCloud.Words
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CreateCloudCommand>(args)
                .MapResult(
                    command => CreateCloudProcessor.Run(command),
                    HandleParseError
                );
        }

        private static int HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                Console.WriteLine(error.ToString() ?? string.Empty, Console.Error);

            return 1;
        }
    }
}