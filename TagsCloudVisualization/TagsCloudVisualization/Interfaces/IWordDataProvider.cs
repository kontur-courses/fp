using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IWordDataProvider
    {
        Result<List<CloudWordData>> GetData(CircularCloudLayouter cloud, List<string> words);
    }
}