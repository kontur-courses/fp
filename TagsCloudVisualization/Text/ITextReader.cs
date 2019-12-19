using System.Collections.Generic;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Text
{
    public interface ITextReader
    {
        HashSet<string> Formats { get; }

        Result<IEnumerable<string>> GetAllWords(string filepath);
    }
}