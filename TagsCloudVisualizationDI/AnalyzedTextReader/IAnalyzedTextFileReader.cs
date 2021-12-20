using System.Collections.Generic;
using System.Text;

namespace TagsCloudVisualizationDI.AnalyzedTextReader
{
    public interface IAnalyzedTextFileReader
    {
        Result<List<string>> ReadText();
    }
}
