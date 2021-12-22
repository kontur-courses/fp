using System.Drawing;
using CommandLine;

namespace TagCloudConsoleUI
{
    [Verb("tag", HelpText = "Set tag options")]
    public class TagOptions
    {
        [Option('f', "font-family", Required = false, HelpText = "Set font-family")]
        public FontFamily FontFamily { get; set; } = FontFamily.GenericSansSerif;
        
        [Option('s', "font-size", Required = false, HelpText = "Set minimum font size")]
        public float FontSize { get; set; } = 15;
    }
}