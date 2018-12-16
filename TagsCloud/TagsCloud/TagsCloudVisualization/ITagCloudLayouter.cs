using System.Collections.Generic;

namespace TagsCloud.TagsCloudVisualization.ColorSchemes.ColorSchemes.SizeDefiners
{
    public interface ITagCloudLayouter
    {
        List<Tag> GetTags(Dictionary<string, int> wordFrequency);
    }
}
