using System.Collections.Generic;
using FunctionalTools;

namespace TagsCloudGenerator.CloudLayouter
{
    public interface ICloudLayouter
    {
        Result<Cloud> LayoutWords(Dictionary<string, int> wordToCount);
    }
}