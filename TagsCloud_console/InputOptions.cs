using CommandLine;

namespace TagsCloud_console
{
    internal class InputOptions
    {
        [Option(Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option(Required = true, HelpText = "Output graphic file name.")]
        public string OutputFile { get; set; }

        [Option(Required = true, HelpText = "Selected filters semicolon separated type names with comma separated settings formatted as 'PropertyName:Value' in parentheses, for example: '--filters=WasteWordsFilter(AllowAdjectives:true);UpperCaseFilter'.")]
        public string Filters { get; set; }

        [Option(Required = true, HelpText = "Selected layouter type name with comma separated settings formatted as 'PropertyName:Value' in parentheses, for example: '--layouter=CircularCloudLayouter'.")]
        public string Layouter { get; set; }

        [Option(Required = true, HelpText = "Selected renderer type name with comma separated settings formatted as 'PropertyName:Value' in parentheses, for example: '--renderer=ColoredRenderer(MinFontSize:50,TagFont:Arial,TagTextColor:Green)'.")]
        public string Renderer { get; set; }
    }
}
