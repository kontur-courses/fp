namespace TagCloud.AppSettings;

public interface IAppSettings
{
    public string InputPath { get; }
    public string OutputPath { get; }
    public string ImageExtension { get; }
    public string FontType { get; }
    public int CloudWidth { get; }
    public int CloudHeight { get; }
    public string LayouterType { get; }
    public int CloudDensity { get; }
    public string ImagePalette { get; }
    public string BackgroundColor { get; }
    public string ForegroundColor { get; }
}