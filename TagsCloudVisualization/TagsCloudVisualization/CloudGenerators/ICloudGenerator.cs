using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization;

public interface ICloudGenerator
{
    CircularCloudLayouter Layouter { get; set; }
    IPreprocessor Preprocessor { get; set; }
    List<TextLabel> GenerateCloud(string text);
}