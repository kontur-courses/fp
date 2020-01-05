using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ResultOf;

namespace TagCloud.Factories
{
    public class FileReader : IFileReader
    {
        public Result<IEnumerable<string>> ReadWordsFromFile(string pathToFile)
        {
            return Result.Of(() =>File.ReadAllLines(pathToFile, Encoding.UTF8)
                .SelectMany(s => Regex.Split(s.ToLower().Trim(), @"\W|_", RegexOptions.IgnoreCase))
                .Where(s => s != string.Empty));
        }
    }
}
