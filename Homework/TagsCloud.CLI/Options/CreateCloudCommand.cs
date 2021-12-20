using CommandLine;

namespace TagsCloud.Words.Options
{
    [Verb("create", HelpText = "generating tags cloud and saving to file")]
    public class CreateCloudCommand : Options
    {
        [Option("dir", HelpText = "Output directory", Default = null)]
        public string OutputDirectory { get; set; }

        [Option("outputfile", HelpText = "Output file name", Default = "result_words")]
        public string OutputFile { get; set; }

        [Option("ext", HelpText = "Output file extension", Default = "png")]
        public string Extension { get; set; }
    }
}