using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TagsCloudVisualizationDI.AnalyzedTextReader
{
    public class DefaultAnalyzedTextFileReader : IAnalyzedTextFileReader
    {
        public DefaultAnalyzedTextFileReader(string preAnalyzedTextPath, Encoding encoding)
        {
            PreAnalyzedTextPath = preAnalyzedTextPath;
            ReadingEncoding = encoding;
        }


        public string PreAnalyzedTextPath { get; }
        public Encoding ReadingEncoding { get; }


        public Result<IEnumerable<string>> ReadText()
        {
            return File.ReadAllLines(PreAnalyzedTextPath, ReadingEncoding);
        }
    }
}
