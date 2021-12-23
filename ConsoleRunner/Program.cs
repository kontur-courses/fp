using CommandLine;
using ResultOf;
using System;
using System.Diagnostics;
using TagCloud2;

namespace ConsoleRunner
{
    public class Program
    {
        public static Result<None> outResult;
        public static void Main(string[] args)
        {
            var options = new Options();
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);
            var generator = new Generator();
            var result = generator.Generate(options);
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
            }
            else
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(options.OutputName) { UseShellExecute = true }
                };
                process.Start();
            }

            outResult = result;
        }
    }
}
