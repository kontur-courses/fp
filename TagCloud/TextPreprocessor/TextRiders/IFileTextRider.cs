using System.Collections.Generic;
using TagCloud.TextPreprocessor.Core;
using TagsCloud;

namespace TagCloud.TextPreprocessor.TextRiders
{
    public interface IFileTextRider
    {
        string[] ReadingFormats { get; }
        TextRiderConfig RiderConfig { get; }
        Result<IEnumerable<Tag>> GetTags();
    }
}