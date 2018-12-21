using System;
using CommandLine;

namespace TagsCloudContainer.Ui
{
    public class ConsoleUi : IUi
    {
        public Options RetrievePaths(string[] args)
        {
            Options options = null;
            var parser = new Parser(with =>
            {
                with.HelpWriter = Console.Out;
                with.IgnoreUnknownArguments = false;
            });

            parser.ParseArguments<Options>(args)
                .WithParsed(o => options = o);

            return options;
        }
    }
}