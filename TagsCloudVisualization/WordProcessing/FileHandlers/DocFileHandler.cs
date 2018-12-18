using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Spire.Doc;

namespace TagsCloudVisualization.WordProcessing.FileHandlers
{
    public class DocFileHandler : IFileHandler
    {
        public string PathToFile { get; }
        public static readonly Regex Regex = new Regex("^.*\\.(doc|docx)$");

        public DocFileHandler(string pathToFile)
        {
            PathToFile = pathToFile;
        }
        public Result<IEnumerable<string>> ReadFile()
        {
            return Result.Ok(new Document())
                .Then(doc => doc.LoadFromFile(PathToFile), "Could not read doc or docx file.")
                .RefineError("One of the external libraries failed.")
                .Then(doc => doc.GetText())
                .Then(text => (IEnumerable<string>)text.Split(new[] { "\r\n" }, StringSplitOptions.None));
        }
    }
}