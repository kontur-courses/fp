using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagCloud.Interfaces;
using TagCloud.Result;

namespace TagCloud
{
    public class FileReader : IFileReader
    {
        private readonly Regex regex = new Regex("[^\\w]?(\\w+)[^\\w]?");

        public Result<IEnumerable<string>> Read(string path)
        {
            return Result.Result.Of(() => new StreamReader(path, Encoding.Default))
                .Then(sr =>
                {
                    var content = sr.ReadToEnd();
                    sr.Dispose();
                    return content;
                })
                .Then(content => regex
                    .Matches(content)
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value));
        }
    }
}