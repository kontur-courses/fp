#region

using System.Collections.Generic;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface IWordPreparator
    {
        Result<IEnumerable<string>> GetPreparedWords(IEnumerable<string> unpreparedWords);
    }
}