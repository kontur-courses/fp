using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class TxtReader : IWordsProvider
    {
        private readonly string filename;
        
        public TxtReader(string filename)
        {
            this.filename = filename;
        }

        public Result<IEnumerable<string>> Provide()
        {
            var result = Result
                .Of(() => new StreamReader(filename, Encoding.UTF8))
                .RefineError($"Failed, opening {filename}")
                .Then(s => ReadStream(s).ToList().AsEnumerable());
        
            return result;
        }

        private IEnumerable<string> ReadStream(StreamReader streamReader)
        {
            using (streamReader)
            {
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine()?.Trim(Environment.NewLine.ToCharArray());
                }
            }
        }
    }
}