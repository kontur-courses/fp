using System.Collections.Generic;

namespace TagsCloudVisualizationDI.AnalyzedTextReader
{
    public interface IAnalyzedTextFileReader
    {
        Result<List<string>> ReadText();
    }
}
