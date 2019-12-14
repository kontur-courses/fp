using System.Collections.Generic;

namespace TagsCloudResult.TextParsing.CloudParsing
{
    public interface ICloudWordsParser
    {
        IEnumerable<CloudWord> Parse();
    }
}