using SixLabors.ImageSharp.Formats;

namespace TagsCloudVisualization;

public interface IVisualizationBuilder
{
    VisualizationBuilder CreateImageFrom(HashSet<WordTagGroup> wordGroups);
    void SaveAs(string filename, IImageEncoder encoder = null);
}