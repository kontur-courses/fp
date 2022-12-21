using CommandLine;
using ResultOf;
using TagCloud.Common;

namespace TagCloud.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(StartApp).WithNotParsed(HandleErrors);
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error.Tag);
            }
        }

        private static void StartApp(Options cmdOptions)
        {
            var visualizationOptions = cmdOptions.MapToVisualizationOptions().OnFail(Console.WriteLine);
            if (!visualizationOptions.IsSuccess)
                return;
            var options = visualizationOptions.GetValueOrThrow();
            var appResult = CloudGeneratorApplication.Run(options).OnFail(Console.WriteLine);
            if (appResult.IsSuccess)
            {
                Console.WriteLine($"Cloud created! Saved to {options.SavingOptions.GetFullSavingPath()}");
            }
        }
    }
}