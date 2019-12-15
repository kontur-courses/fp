using System;
using System.Linq;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;
using Xceed.Words.NET;
using FailuresProcessing;

namespace TagsCloudGeneratorExtensions
{
    [Factorial("DocxLinesParser")]
    public class DocxLinesParser : IWordsParser
    {
        public Result<string[]> ParseFromFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();
            return
                Result.Of(() => DocX.Load(filePath).Paragraphs.Select(p => p.Text))
                .Then(strs => Result.Ok(strs.Where(s => s.Length > 0).ToArray()))
                .RefineError($"Failed to read file by \'{filePath}\' path")
                .RefineError($"{nameof(DocxLinesParser)} failure");
        }
    }
}