using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;


namespace TagsCloudVisualization.WordsFileReading
{
    public class LinesParser : IParser
    {
        public Result<IEnumerable<string>> ParseText(string text)
        {
            return Result.Ok(
                Regex.Split(text, Environment.NewLine)
                .Where(w => w != "")
                .Select(w => w.Trim()));
        }

        public string GetModeName()
        {
            return "lines";
        }
    }
}
