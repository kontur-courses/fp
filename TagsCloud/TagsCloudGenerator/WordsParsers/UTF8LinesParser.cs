using FailuresProcessing;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.WordsParsers
{
    [Factorial("UTF8LinesParser")]
    public class UTF8LinesParser : IWordsParser
    {
        public Result<string[]> ParseFromFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            return 
                Result.Of(() => File.ReadAllLines(filePath, Encoding.UTF8))
                .Then(lines => Result.Ok(lines.Where(l => l.Length > 0).ToArray()))
                .RefineError($"Failed to read file by \'{filePath}\' path")
                .RefineError($"{nameof(UTF8LinesParser)} failure");
        }
    }
}