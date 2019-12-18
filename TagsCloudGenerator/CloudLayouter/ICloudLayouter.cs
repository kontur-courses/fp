using System.Collections.Generic;

namespace TagsCloudGenerator.CloudLayouter
{
    public interface ICloudLayouter
    {
        Result<Cloud> LayoutWords(Dictionary<string, int> wordToCount);
    }
}