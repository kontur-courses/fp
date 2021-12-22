using CommandLine;

namespace CTV.ConsoleInterface.Options
{
    [Verb("visualize")]
    public class VisualizeCommand
    {
        [Option("inputFile", Required = true, HelpText = "Set path to input txt file")]
        public string InputFile { get; set; }

        [Option("OutputFile", Required = true, HelpText = "Set path where image will be saved")]
        public string OutputFile { get; set; }

        [Option("textFormat", Default = "txt", HelpText = "Set format for input file")]
        public string TextFormat { get; set; }

        [Option("imageFormat", Default = "png", HelpText = "Set format for image saving")]
        public string ImageFormat { get; set; }

        [Option("backgroundColor", Default = "2F2F2F", HelpText = "Set background color")]
        public string BackgroundColor { get; set; }

        [Option("textColor", Default = "D2F898", HelpText = "Set text color")]
        public string TextColor { get; set; }

        [Option("strokeColor", Default = "0000FF", HelpText = "Set text stroke color")]
        public string StrokeColor { get; set; }

        [Option("width", Default = 1920, HelpText = "Set image width")]
        public int Width { get; set; }

        [Option("height", Default = 1080, HelpText = "Set image height")]
        public int Height { get; set; }

        [Option("fontName", Default = "Arial")]
        public string FontName { get; set; }

        [Option("fontSize", Default = 240)] public int FontSize { get; set; }


        public VisualizerOptions ToVisualizerOptions()
        {
            return new VisualizerOptions(
                inputFile: InputFile,
                outputFile: OutputFile,
                imageWidth: Width,
                imageHeight: Height,
                fontName: FontName,
                fontSize: FontSize,
                backgroundColorArgb: BackgroundColor,
                textColorArgb: TextColor,
                strokeColorArgb: StrokeColor,
                savingFormat: ImageFormat,
                inputFileFormat: TextFormat
            );
        }
    }
}