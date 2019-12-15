using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer.Core
{
    public class Parameters
    {
        public string InputFilePath { get; }

        public string OutputFilePath { get; }

        public List<Color> Colors { get; }

        public Font Font { get;}

        public Size ImageSize { get; }
        public ImageFormat ImageFormat { get; }

        public Parameters(string inputFilePath, string outputFilePath, List<Color> colors, Font font, Size imageSize,
            ImageFormat imageFormat)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            Colors = colors;
            Font = font;
            ImageSize = imageSize;
            ImageFormat = imageFormat;
        }

        public Parameters() { }

        public Parameters WithInputFilePath(string inputFilePath)
        {
            return new Parameters(inputFilePath, OutputFilePath, Colors, Font, ImageSize, ImageFormat);
        }

        public Parameters WithOutputFilePath(string outputFilePath)
        {
            return new Parameters(InputFilePath, outputFilePath, Colors, Font, ImageSize, ImageFormat);
        }

        public Parameters WithColors(List<Color> colors)
        {
            return new Parameters(InputFilePath, OutputFilePath, colors, Font, ImageSize, ImageFormat);
        }

        public Parameters WithFont(Font font)
        {
            return new Parameters(InputFilePath, OutputFilePath, Colors, font, ImageSize, ImageFormat);
        }

        public Parameters WithImageSize(Size imageSize)
        {
            return new Parameters(InputFilePath, OutputFilePath, Colors, Font, imageSize, ImageFormat);
        }

        public Parameters WithImageFormat(ImageFormat imageFormat)
        {
            return new Parameters(InputFilePath, OutputFilePath, Colors, Font, ImageSize, imageFormat);
        }
    }
}