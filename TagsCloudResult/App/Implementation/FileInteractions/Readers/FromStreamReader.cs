using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using App.Infrastructure.FileInteractions.Readers;

namespace App.Implementation.FileInteractions.Readers
{
    public class FromStreamReader : ILinesReader
    {
        private readonly StreamReader streamReader;

        public FromStreamReader(StreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public Result<IEnumerable<string>> ReadLines()
        {
            var resultOfReading = Result.Of(() => ReadFromStream()
                .SelectMany(line => Regex.Split(line, @"\P{L}+", RegexOptions.Compiled))
                .Select(word => word));

            return resultOfReading.IsSuccess
                ? resultOfReading
                : resultOfReading.RefineError("Can not read lines from stream");
        }

        private IEnumerable<string> ReadFromStream()
        {
            using (streamReader)
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrWhiteSpace(line))
                {
                    yield return line;
                    line = streamReader.ReadLine();
                }
            }
        }
    }
}