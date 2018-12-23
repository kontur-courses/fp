using System.Collections.Generic;
using System.IO;

namespace CloodLayouter.Infrastructer
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<string> ReadLines(this StreamReader streamReader)
        {
            var line = streamReader.ReadLine();
            while (line != null)
            {
                yield return line;
                line = streamReader.ReadLine();
            }
        }
    }
}