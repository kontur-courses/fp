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
        public readonly FontFamily fontFamily;
        public readonly ImageFormat imageFormat;
        public readonly string[] ignoredPartOfSpeech;
        public readonly GenerationsAlgorithm generationAlgorithmName;
        public readonly TextSplitter splitterName;
        public readonly SupportedTypes.ColorSchemes colorSchemeName;
        public readonly string pathToMystem;

        public TagCloudSettings(string pathToInput,
            string pathToOutput,
            string pathToBoringWords,
            int widthOutputImage,
            int heightOutputImage,
            Color backgroundColor,
            FontFamily fontFamily,
            string[] ignoredPartOfSpeech,
            GenerationsAlgorithm generationAlgorithmName,
            TextSplitter splitterName,
            SupportedTypes.ColorSchemes colorSchemeName,
            string pathToMystem,
            ImageFormat imageFormat)
        {
            this.pathToInput = pathToInput;
            this.pathToOutput = pathToOutput;
            this.pathToBoringWords = pathToBoringWords;
            this.widthOutputImage = widthOutputImage;
            this.heightOutputImage = heightOutputImage;
            this.backgroundColor = backgroundColor;
            this.fontFamily = fontFamily;
            this.imageFormat = imageFormat;
            this.ignoredPartOfSpeech = ignoredPartOfSpeech;
            this.generationAlgorithmName = generationAlgorithmName;
            this.splitterName = splitterName;
            this.colorSchemeName = colorSchemeName;
            this.pathToMystem = pathToMystem;
        }
    }
}
