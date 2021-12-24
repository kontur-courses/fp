using System;
using System.Collections.Generic;
using System.IO;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.Parsers
{
    public class TxtParser : IParser
    {
        public Result<IEnumerable<string>> ParseWords(string filePath) =>
            Result.Of(() => new StreamReader(filePath))
                .Then(x => x.ReadToEnd())
                .Then(x =>
                    x.Split(new[] {Environment.NewLine, " "}, StringSplitOptions.RemoveEmptyEntries))
                .Then(x => x as IEnumerable<string>)
                .RefineError("Wrong file path");
    }
}