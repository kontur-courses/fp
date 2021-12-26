using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IWordPreparator
    {
        Result<IEnumerable<string>> GetPreparedWords(IEnumerable<string> unpreparedWords);
    }
}