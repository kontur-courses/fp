using System.Collections.Generic;
using System.Drawing;
using CommandLine;

#pragma warning disable 8618 Non-nullable member is uninitialized
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace TagsCloudApp
{
    [Verb("render")]
    public class RenderArgs : IRenderArgs
    {
        [Option('i', "input", Default = "input.txt")]
        public string InputPath { get; set; }

        [Option('o', "output", Default = "output.png")]
        public string OutputPath { get; set; }

        [Option("fontFamily", Default = "Courier New")]
        public string FontFamily { get; set; }

        [Option("maxFont", Default = 32)]
        public int MaxFontSize { get; set; }

        [Option("minFont", Default = 8)]
        public int MinFontSize { get; set; }

        [Option('s', "size", Default = null, HelpText = "Example: 10, 10")]
        public Size? ImageSize { get; set; }

        [Option("scale", Default = 1.0f)]
        public float ImageScale { get; set; }

        [Option("background", Default = "Transparent")]
        public string BackgroundColor { get; set; }

        [Option("color", Default = "Aqua")]
        public string DefaultColor { get; set; }

        [Option("colorMapper", HelpText = "Determines how words will be colored", Default = "speechPart")]
        public string ColorMapperType { get; set; }

        [Option("speechPartColorMap", Default = "(S Aqua)(V Red)(A Blue)")]
        public string SpeechPartColorMap { get; set; }

        [Option("imageFormat", Default = "png")]
        public string ImageFormat { get; set; }

        [Option("wordsScale", Default = "linear")]
        public string WordsScale { get; set; }

        [Option("ignoreSpeechParts", Separator = ' ', Default = new[] {"INTJ", "PART", "PR", "CONJ"})]
        public IEnumerable<string> IgnoredSpeechParts { get; set; }
    }
}