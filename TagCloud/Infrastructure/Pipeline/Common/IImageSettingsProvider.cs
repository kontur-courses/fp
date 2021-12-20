namespace TagCloud.Infrastructure.Pipeline.Common;

public interface IImageSettingsProvider
{
    int ImageWidth { get; }
    int ImageHeight { get; }
}