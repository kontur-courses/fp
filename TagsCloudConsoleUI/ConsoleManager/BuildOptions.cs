using System;
using System.Drawing;
using TagsCloudGenerator;

namespace TagsCloudConsoleUI
{
    public class BuildOptions
    {
        public readonly string InputFilePath;
        public readonly string OutputFilePath;
        public readonly string ImageExtension;

        public readonly int Width;
        public readonly int Height;

        public readonly Color[] ColorsPalette;
        public readonly Color BackgroundColor;

        public readonly float SpiralStep;

        public readonly FontFamily FontFamily;
        public readonly int FontSizeMultiplier;
        public readonly int MaximalFontSize;

        public readonly string[] BoringPartsOfSpeech;
        public readonly string[] BoringWords;

        public BuildOptions(ConsoleParsedOptions parsedOptions)
        {
            InputFilePath = parsedOptions.InputFilePath;
            OutputFilePath = parsedOptions.OutputFilePath;
            ImageExtension = parsedOptions.ImageExtension;

            if(parsedOptions.Width < 0)
                throw new ArgumentOutOfRangeException($"Image width can't be less than zero");
            if (parsedOptions.Height < 0)
                throw new ArgumentOutOfRangeException($"Image heigth can't be less than zero");
            Width = parsedOptions.Width;
            Height = parsedOptions.Height;

            BackgroundColor = ColorsHexConverter.CreateFromHex(parsedOptions.BackgroundColor);
            ColorsPalette = ColorsHexConverter.CreateFromHexEnumerable(parsedOptions.ColorsPalette);

            SpiralStep = parsedOptions.SpiralStep;

            FontFamily = new FontFamily(parsedOptions.FontFamilyName);
            FontSizeMultiplier = parsedOptions.FontSizeMultiplier;
            if (parsedOptions.MaximalFontSize <= 0)
                throw new ArgumentOutOfRangeException($"Maximal font size can't be less or equal than zero");
            MaximalFontSize = parsedOptions.MaximalFontSize;

            BoringPartsOfSpeech = parsedOptions.BoringPartsOfSpeech.Split(' ');
            BoringWords = parsedOptions.BoringWords.Split(' ');
        }
    }
}