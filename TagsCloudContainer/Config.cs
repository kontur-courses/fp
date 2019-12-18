using System;
using System.Collections.Generic;
using System.IO;
using ResultOf;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class Config
    {
        public string InputFilePath { get; }
        public string OutputFilePath { get; }
        public Size LayoutSize { get; }
        public Color FontAndBorderColor { get; }
        public Font TextFont { get; }
        public bool CompressionFlag { get; }
        public string FileFormat { get; }
        public IEnumerable<string> ExcludedParts { get; }

        private static string[] acceptableFormats = new string[]
        {
            "memorybmp", "bmp", "emf", "wmf", "gif", "jpeg", "png",
            "tiff", "exif", "icon"
        };

        private static string[] acceptableParts = new string[]
        {
            "A", "ADV", "ADVPRO", "ANUM", "APRO", "COM", "CONJ", "INTJ",
            "NUM", "PART", "PR", "S", "SPRO", "V"
        };

        public Config(string inputFilePath, string outputFilePath, Size layoutSize, Color color, Font font,
            bool compressionFlag, string fileFormat, IEnumerable<string> excludedParts)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            LayoutSize = layoutSize;
            FontAndBorderColor = color;
            TextFont = font;
            CompressionFlag = compressionFlag;
            FileFormat = fileFormat;
            ExcludedParts = excludedParts;
        }

        public static Result<Config> FromArguments(CMDOptions O)
        {
            return FromArguments(O.InputFile, O.OutputFile, O.Width, O.Height, O.Color, O.Font,
                O.Compression, O.Format, O.Excluded);
        }

        private static bool DoesFontExist(string fontFamilyName, FontStyle fontStyle)
        {
            bool result;

            try
            {
                using (FontFamily family = new FontFamily(fontFamilyName))
                    result = family.IsStyleAvailable(fontStyle);
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        private static Result<Config> FromArguments(string inpPath, string outPath, int width, int height, string color,
            string font, bool compFlag, string fileFormat, IEnumerable<string> excluded)
        {
            if (!File.Exists(inpPath))
                return Result.Fail<Config>("Input file doesn't exists!");
            if (width < 0 || height < 0)
                return Result.Fail<Config>("Width and Height should be positive numbers");
            Color userColor;
            userColor = Color.FromName(color);
            if (!userColor.IsKnownColor)
                return Result.Fail<Config>("Unrecognized color, try another.");

            Font userFont;
            if (!DoesFontExist(font, FontStyle.Regular))
                return Result.Fail<Config>("Unrecognized font, try another.");
            userFont = new Font(font, 50);

            if (!acceptableFormats.Contains(fileFormat.ToLower()))
            {
                return Result.Fail<Config>("Unacceptable format of output file");
            }

            foreach (var part in excluded)
            {
                if (!acceptableParts.Contains(part.ToUpper()))
                {
                    return Result.Fail<Config>("Unrecognized part of speech to exclude");
                }
            }

            return Result.Ok(new Config(inpPath, outPath, new Size(width, height), userColor,
                userFont, compFlag, fileFormat, excluded));
        }
    }
}