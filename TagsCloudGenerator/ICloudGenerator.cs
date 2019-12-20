using System.Collections.Generic;
using FunctionalTools;
using TagsCloudGenerator.CloudLayouter;

namespace TagsCloudGenerator
{
    public interface ICloudGenerator
    {
        Result<Cloud> Generate(Dictionary<string, int> wordsToCount);
    }
}