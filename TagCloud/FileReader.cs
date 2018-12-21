using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagCloud.Interfaces;

namespace TagCloud
{
    public class FileReader : IFileReader
    {
        private readonly Regex regex = new Regex("[^\\w]?(\\w+)[^\\w]?");

        public Result<IEnumerable<string>> Read(string path)
        {
            return Result.Of(() => File.ReadAllText(path, Encoding.Default))
                .Then(content => regex
                    .Matches(content)
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value));
        }
    }
}