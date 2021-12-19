using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TagsCloudVisualizationDI.AnalyzedTextReader
{
    public interface ITextFileReader
    {
        public string PreAnalyzedTextPath { get; }

        public Encoding ReadingEncoding { get; }
        Result<IEnumerable<string>> ReadText();
    }
}
