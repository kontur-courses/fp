using System.Collections.Generic;
using System.Text;

namespace TagsCloudVisualizationDI.AnalyzedTextReader
{
    public interface IAnalyzedTextFileReader
    {
        Result<IEnumerable<string>> ReadText();
    }
}
