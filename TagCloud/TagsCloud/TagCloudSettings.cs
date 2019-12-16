using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud.SupportedTypes;

namespace TagsCloud
{
    public class TagCloudSettings
    {
        public readonly string pathToInput;
        public readonly string pathToOutput;
        public readonly string pathToBoringWords;
        public readonly int widthOutputImage;
        public readonly int heightOutputImage;
        public readonly Color backgroundColor;
        public readonly string fontName;
        public readonly ImageFormat imageFormat;
        public readonly string[] ignoredPartOfSpeech;
        public readonly GenerationsAlghoritm generationAlgoritmName;
        public readonly TextSpliter spliterName;
        public readonly SupportedTypes.ColorSchemes colorSchemeName;
        public readonly string pathToMystem;

        public TagCloudSettings(string PathToInput,
            string PathToOutput,
            string boringWords,
            int WidthOutputImage,
            int HeightOutputImage,
            Color BackgroundColor,
            string FontName,
            string[] ignoredPartOfSpeech,
            GenerationsAlghoritm generationAlgoritm,
            TextSpliter splitType,
            SupportedTypes.ColorSchemes colorScheme,
            string pathToMystem,
            ImageFormat imageFormat)
        {
            this.pathToInput = PathToInput;
            this.pathToOutput = PathToOutput;
            this.pathToBoringWords = boringWords;
            this.widthOutputImage = WidthOutputImage;
            this.heightOutputImage = HeightOutputImage;
            this.backgroundColor = BackgroundColor;
            this.fontName = FontName;
            this.imageFormat = imageFormat;
            this.ignoredPartOfSpeech = ignoredPartOfSpeech;
            this.generationAlgoritmName = generationAlgoritm;
            this.spliterName = splitType;
            this.colorSchemeName = colorScheme;
            this.pathToMystem = pathToMystem;
        }
    }
}
