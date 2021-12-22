namespace TagCloud.Settings
{
    public interface IProcessorSettings : IDrawingSettings
    {
        public string ExcludedWordsFile { get; }
        public string InputFilename { get; }
        public string OutputFilename { get; }
        public string TargetDirectory { get; }
    }
}
