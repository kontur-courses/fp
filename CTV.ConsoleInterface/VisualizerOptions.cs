using System;
using System.Drawing;
using CTV.Common.VisualizerContainer;
using FunctionalProgrammingInfrastructure;

namespace CTV.ConsoleInterface
{
    public class VisualizerOptions
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public Size ImageSize { get; set; }
        public Font Font { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color StrokeColor { get; set; }
        public SavingFormat SavingFormat { get; set; }
        public InputFileFormat InputFileFormat { get; set; }
        
        public VisualizerOptions(
            string inputFile,
            string outputFile,
            int imageWidth,
            int imageHeight,
            string fontName,
            int fontSize,
            string backgroundColorArgb,
            string textColorArgb,
            string strokeColorArgb,
            string savingFormat,
            string inputFileFormat
            )
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            InitSize(imageWidth, imageHeight);
            InitFont(fontName, fontSize);
            BackgroundColor = ColorFromHex(backgroundColorArgb);
            TextColor = ColorFromHex(textColorArgb);
            StrokeColor = ColorFromHex(strokeColorArgb);
            SavingFormat = ParseSavingFormat(savingFormat);
            InputFileFormat = ParseTextFormat(inputFileFormat);
        }

        private void InitSize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new Exception("Image size must be with positive width and height");
            ImageSize = new Size(width, height);
        }

        private void InitFont(string fontName, int fontSize)
        {
            if (fontSize <= 0)
                throw new Exception("font size must be positive");
            var font = new Font(fontName, fontSize);
            if (font.Name != fontName)
                throw new Exception($"Was not able to find font with name {fontName}");
            Font = font;

        }

        private static Color ColorFromHex(string hexed)
        {
            return Result.Ok(Color.FromArgb(0, 0, 0))
                .Then(color => color.WithRed(Convert.ToInt32(hexed[..2], 16)))
                .Then(color => color.WithGreen(Convert.ToInt32(hexed[2..4], 16)))
                .Then(color => color.WithBlue(Convert.ToInt32(hexed[4..], 16)))
                .ReplaceError(e => $"Invalid color {hexed}")
                .GetValueOrThrow();
        }

        private static SavingFormat ParseSavingFormat(string format)
        {
            if (Enum.TryParse(format, ignoreCase:true, out SavingFormat result))
                return result;
            throw new ArgumentException($"Unknown saving format: {format}");
        }
        
        private static InputFileFormat ParseTextFormat(string format)
        {
            if (Enum.TryParse(format, ignoreCase:true, out InputFileFormat result))
                return result;
            throw new ArgumentException($"Unknown Input File Format: {format}");
        }
    }
}