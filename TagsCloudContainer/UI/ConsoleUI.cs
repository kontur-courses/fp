using CommandLine;

namespace TagsCloudContainer.Ui
{
    public class ConsoleUi : IUi
    {
        public Options RetrievePaths(string[] args)
        {
            Options result = null;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => { result = o; });

            return result;
        }
    }

    public class Options
    {
        [Option("text", Default = "Resources\\sample.txt", Required = false, HelpText = "File to read text from")]
        public string TextFile { get; set; }

        [Option("image", Default = "result.png", Required = false, HelpText = "Output image file")]
        public string ImageFile { get; set; }
    }
}