using CommandLine;

namespace TagsCloudContainer.Ui
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "File to read text from", MetaName = "text")]
        public string TextFile { get; set; }

        [Value(1, Required = true, HelpText = "Output image file", MetaName = "img")]
        public string ImageFile { get; set; }

        [Value(2, Required = true, HelpText = "Settings JSON file", MetaName = "json")]
        public string SettingsFile { get; set; }

        [Option(HelpText = "Use common words filter")]
        public bool Common { get; set; }

        [Option("blacklist", HelpText = "Words' black list file")]
        public string BlackListFile { get; set; }

        [Option("initial", HelpText = "Use initial form converter")]
        public bool InitialForm { get; set; }
    }
}