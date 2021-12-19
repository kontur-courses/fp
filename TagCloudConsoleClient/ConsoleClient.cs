using System.Diagnostics;
using System.IO;
using CommandLine;
using TagsCloudVisualization;

namespace TagCloudConsoleClient
{
    public class ConsoleClient
    {
        private readonly TextWriter console;

        public ConsoleClient(TextWriter console)
        {
            this.console = console;
        }

        public void Start(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Start);
        }

        private void Start(Options options)
        {
            var factory = new TagCloudFactory();
            var result = factory.CreateInstance(options.Manhattan, options.Order)
                .Then(tagCloud => tagCloud.CreateTagCloudFromFile(options.SourcePath, options.ResultPath, options.Font,
                    options.BackGround, options.MaxCount, options.Width, options.Height));
            if (!result.IsSuccess)
            {
                console.WriteLine("Error: " + result.Error);
                return;
            }
            console.WriteLine("Success!");
            if (options.OpenResult)
            {
                console.WriteLine("Opening result");
                Process.Start(new ProcessStartInfo(options.ResultPath)
                    { UseShellExecute = true });
            }
        }
    }
}