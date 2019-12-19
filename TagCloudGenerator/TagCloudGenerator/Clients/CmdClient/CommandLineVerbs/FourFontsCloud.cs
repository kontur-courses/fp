using CommandLine;
using TagCloudGenerator.GeneratorCore.TagClouds;

// used implicitly by CommandLine lib
// ReSharper disable ClassNeverInstantiated.Global

namespace TagCloudGenerator.Clients.CmdClient.CommandLineVerbs
{
    [Verb("FourFontsCloud", HelpText = "All tags will divide into four font-groups.")]
    public class FourFontsCloud<TWebCloud> : TagCloudOptions<TWebCloud> where TWebCloud : WebCloud
    {
        public override string ImageFilename => "WebTagCloud.png";
        public override int GroupsCount => 4;

        [Option("mutual_font",
                Default = "Bahnschrift SemiLight",
                HelpText = "All words will have the same font type (but can have different sizes).")]
        public override string MutualFont { get; internal set; }

        [Option("background_color",
                Default = "#FF00222B",
                HelpText = "Background color of cloud image.")]
        public override string BackgroundColor { get; internal set; }

        [Option("font_sizes",
                Default = "60_22_18_13",
                HelpText = "Font sizes for each tags group.")]
        public override string FontSizes { get; internal set; }

        [Option("tag_colors",
                Default = "#FFFFFFFF_#FFFF6600_#FFD45500_#FFA05A2C",
                HelpText = "Tag colors for each tags group.")]
        public override string TagColors { get; internal set; }
    }
}