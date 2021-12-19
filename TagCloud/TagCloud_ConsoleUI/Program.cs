using System;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;
using ResultOf;
using TagCloud;

namespace TagCloud_ConsoleUI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var argumentsPattern = new Regex("(\"[^\"]*\")|(\\S+)", RegexOptions.Compiled);
            var builder = new TagCloudBuilder();
            var tagCloud = builder
                .CreateDefault()
                .WithStatusWriter<ConsoleStatusWriter>()
                .Build();
            var parser = new Parser(c => c.HelpWriter = null);
            while (!args.Contains("exit"))
            {
                Result.Of(() => parser.ParseArguments<DrawerOptions, TextProcessingOptions, ClearOptions>(args),
                        "При разборе аргументов произошла ошибка")
                    .Then(res =>
                        res.MapResult(
                            (DrawerOptions opts) => tagCloud.DrawTagClouds(opts),
                            (TextProcessingOptions opts) => tagCloud.ProcessText(opts),
                            (ClearOptions opts) => tagCloud.ClearProcessedTexts(),
                            errors => DisplayHelp(res)))
                    .OnFail(Console.WriteLine);

                args = argumentsPattern.Matches(Console.ReadLine())
                    .Select(m => m.Value.Replace("\"", ""))
                    .ToArray();
            }
        }

        private static object DisplayHelp(ParserResult<object> parserResult)
        {
            Console.WriteLine(HelpText.AutoBuild(parserResult, help =>
            {
                help.AdditionalNewLineAfterOption = false;
                help.AddEnumValuesToHelpText = true;
                help.Heading = "TagCloud Console UI\n";
                help.Copyright = string.Empty;
                help.AddPreOptionsText("Пример взаимодействия:\n" +
                                       "process -p dataSample.txt\n" +
                                       "draw");
                return help;
            }));
            return null;
        }
    }
}