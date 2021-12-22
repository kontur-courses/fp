using System.Collections.Generic;

namespace TagCloud.Settings
{
    public class ProcessorSettings : IProcessorSettings
    {
        public IEnumerable<string> PenColors { get; }
        public string BackgroundColor { get; }
        public string FontName { get; }
        public int FontSize { get; }
        public int Width { get; }
        public int Height { get; }
        public string AlgorithmName { get; }
        public string ExcludedWordsFile { get; }
        public string InputFilename { get; }
        public string OutputFilename { get; }
        public string TargetDirectory { get; }

        public ProcessorSettings(IEnumerable<string> penColor, 
            string backgroundColor, 
            string fontName,
            int fontSize,
            int width, 
            int height, 
            string algorithmName, 
            string excludedWordsFile, 
            string inputFilename, 
            string outputFilename, 
            string targetDirectory)
        {
            PenColors = penColor;
            BackgroundColor = backgroundColor;
            FontName = fontName;
            FontSize = fontSize;
            Width = width;
            Height = height;
            AlgorithmName = algorithmName;
            ExcludedWordsFile = excludedWordsFile;
            InputFilename = inputFilename;
            OutputFilename = outputFilename;
            TargetDirectory = targetDirectory;
        }
    }
}
