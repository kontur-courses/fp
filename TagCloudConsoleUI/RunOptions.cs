using CommandLine;

namespace TagCloudConsoleUI
{
    [Verb("run", HelpText = "Set input and output file's paths and execute visualize")]
    public class RunOptions
    {
        [Option('i', "input-path", Required = true, HelpText = "Set path to input file with words")]
        public string InputFilePath { get; set; } = "";
        
        [Option('o', "output-path", Required = true, HelpText = "Set path to save image")]
        public string OutputFilePath { get; set; } = "";
    }
}