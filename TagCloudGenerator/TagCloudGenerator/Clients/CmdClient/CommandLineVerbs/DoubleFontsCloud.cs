using CommandLine;
using TagCloudGenerator.GeneratorCore.TagClouds;

// used implicitly by CommandLine lib (CommandLineClient.GetOptions())
// ReSharper disable ClassNeverInstantiated.Global

namespace TagCloudGenerator.Clients.CmdClient.CommandLineVerbs
{
    [Verb("DoubleFontsCloud", HelpText = "All tags except central will have same text font.")]
    public class DoubleFontsCloud<TCommonWordsCloud> : TagCloudOptions<TCommonWordsCloud>
        where TCommonWordsCloud : CommonWordsCloud
    {
        public override string ImageFilename => "CommonWordsTagCloud.png";
        public override int GroupsCount => 2;

        [Option("mutual_font",
                Default = "Bahnschrift SemiLight",
                HelpText = "All words will have the same font type (but can have different sizes).")]
        public override string MutualFont { get; internal set; }

        [Option("background_color",
                Default = "#FFFFFFFF",
                HelpText = "Background color of cloud image.")]
        public override string BackgroundColor { get; internal set; }

        [Option("font_sizes",
                Default = "30_18",
                HelpText = "Font sizes for each tags group.")]
        public override string FontSizes { get; internal set; }

        [Option("tag_colors",
                Default = "#FF000000_#FF000000",
                HelpText = "Tag colors for each tags group.")]
        public override string TagColors { get; internal set; }
    }
}