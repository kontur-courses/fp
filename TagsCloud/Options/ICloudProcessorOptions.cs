using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloud.Entities;
using TagsCloudVisualization;

namespace TagsCloud.Options;

public interface ICloudProcessorOptions
{
    ColoringStrategy ColoringStrategy { get; }
    Color[] Colors { get; }
    FontFamily FontFamily { get; }
    ILayout Layout { get; }
    MeasurerType MeasurerType { get; }
    SortType Sort { get; }
    int MaxFontSize { get; }
    int MinFontSize { get; }
}