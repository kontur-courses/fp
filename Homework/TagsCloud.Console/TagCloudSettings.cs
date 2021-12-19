using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommandLine;
using TagsCloudContainer;
using TagsCloudContainer.Layouter.PointsProviders;
using TagsCloudContainer.Visualizer.ColorGenerators;
using TagsCloudContainer.WordsPreparator;

namespace TagsCloud.Console
{
    public class TagCloudSettings : ITagCloudSettings
    {
        [Option('h', "imageHeight", Default = 1080, HelpText = "Set image height")]
        public int ImageHeight { get; set; }

        [Option('w', "imageWidth", Default = 1920, HelpText = "Set image width")]
        public int ImageWidth { get; set; }

        [Option('s', "fontSize", Default = 20, HelpText = "Set font size")]
        public int FontSize { get; set; }

        [Option('l', "minWordsLength", Default = 3, HelpText = "Filter words by length")]
        public int MinWordLength { get; set; }

        [Option('p', "excludingSpeechParts", Separator = ',', HelpText = "Speech parts to exclude(separated by comma)")]
        public ICollection<SpeechPart> SelectedSpeechParts { get; set; } = null!;

        [Option('a', "layoutAlgorythm", Default = LayoutAlrogorithm.Circular, HelpText = "Set layouting algorythm")]
        public LayoutAlrogorithm LayoutAlgorythm { get; set; }

        [Option('c', "coloringAlgorythm", Default = PalleteType.Random, HelpText = "Set coloring algorythm")]
        public PalleteType ColoringAlgorythm { get; set; }

        [Option('f', "fontName", Default = "Arial", HelpText = "Set font family")]
        public string FontName { get; set; } = null!;

        [Option('b', "backgroundColor", Default = "White", HelpText = "Set background color")]
        public string BackgroundColor { get; set; } = null!;


        public static Result<TagCloudSettings> Parse(string[] args)
        {
            return ArgumentsParser.Parse<TagCloudSettings>(args)
                .Then(ValidateFontFamily)
                .Then(ValidateBackgroundColor)
                .Then(ValidateFontSize)
                .Then(ValidateImageSize)
                .RefineError("Option is invalid");
        }

        private static Result<TagCloudSettings> ValidateFontFamily(TagCloudSettings settings)
        {
            return FontFamily.Families
                .Select(f => f.Name.ToLower())
                .Contains(settings.FontName.ToLower())
                ? Result.Ok(settings)
                : Result.Fail<TagCloudSettings>($"Font Family {settings.FontName} is not found");
        }

        private static Result<TagCloudSettings> ValidateFontSize(TagCloudSettings settings)
        {
            return settings.FontSize > 0
                ? Result.Ok(settings)
                : Result.Fail<TagCloudSettings>("Font size should not be negative");
        }

        private static Result<TagCloudSettings> ValidateImageSize(TagCloudSettings settings)
        {
            return settings.ImageHeight > 0 && settings.ImageWidth > 0
                ? Result.Ok(settings)
                : Result.Fail<TagCloudSettings>("Image size should not be negative");
        }

        private static Result<TagCloudSettings> ValidateBackgroundColor(TagCloudSettings settings)
        {
            return Color.FromName(settings.BackgroundColor).IsKnownColor
                ? Result.Ok(settings)
                : Result.Fail<TagCloudSettings>($"Color {settings.BackgroundColor} does not exist");
        }
    }
}