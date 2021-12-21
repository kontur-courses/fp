using System;
using System.Drawing;
using CTV.Common.VisualizerContainer;

namespace CTV.ConsoleInterface.Options
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
            ImageSize = new Size(imageWidth, imageHeight);
            Font = new Font(fontName, fontSize);
            BackgroundColor = ColorFromHex(backgroundColorArgb);
            TextColor = ColorFromHex(textColorArgb);
            StrokeColor = ColorFromHex(strokeColorArgb);
            SavingFormat = ParseSavingFormat(savingFormat);
            InputFileFormat = ParseTextFormat(inputFileFormat);
        }

        private static Color ColorFromHex(string hexed)
        {
            if (hexed.Length != 6)
                throw new ArgumentException($"Invalid color {hexed}");
            var red = Convert.ToInt32(hexed[0..2], 16);
                var green = Convert.ToInt32(hexed[2..4], 16);
                var blue = Convert.ToInt32(hexed[4..6], 16);
                try
                {
                    return Color.FromArgb(red, green, blue);
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Color was in icorrect format {hexed}", e);
                }
        }

        private static SavingFormat ParseSavingFormat(string format)
        {
            if (Enum.TryParse(format, ignoreCase:true, out SavingFormat result))
                return result;
            throw new ArgumentException($"Saving unknown saving format {format}");
        }
        
        private static InputFileFormat ParseTextFormat(string format)
        {
            if (Enum.TryParse(format, ignoreCase:true, out InputFileFormat result))
                return result;
            throw new ArgumentException($"Saving unknown saving format {format}");
        }
    }
}