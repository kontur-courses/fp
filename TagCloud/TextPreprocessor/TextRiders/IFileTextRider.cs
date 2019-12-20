using System.Collections.Generic;
using ResultLogic;
using TagCloud.TextPreprocessor.Core;

namespace TagCloud.TextPreprocessor.TextRiders
{
    public interface IFileTextRider
    {
        string[] ReadingFormats { get; }
        TextRiderConfig RiderConfig { get; }
        Result<IEnumerable<Tag>> GetTags();
    }
}