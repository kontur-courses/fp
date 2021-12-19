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


        public IEnumerable<string> ReadText()
        {
            return File.ReadAllLines(PreAnalyzedTextPath, ReadingEncoding);


            /*
            if (File.Exists(PreAnalyzedTextPath))
            {
                return Result.Ok(File.ReadAllLines(PreAnalyzedTextPath, ReadingEncoding) as IEnumerable<string>);
            }

            //throw new FileNotFoundException($"Giving path {PreAnalyzedTextPath} is not valid");
            return Result.Fail<IEnumerable<string>>($"Giving path {PreAnalyzedTextPath} is not valid");
            */
        }
    }
}
