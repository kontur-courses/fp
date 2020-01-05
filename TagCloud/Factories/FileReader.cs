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
        private readonly Regex regex;

        public FileReader()
        {
            regex = new Regex(@"\W|_", RegexOptions.IgnoreCase);
        }
        public Result<IEnumerable<string>> ReadWordsFromFile(string pathToFile)
        {
            return Result.Of(() =>File.ReadAllLines(pathToFile, Encoding.UTF8)
                .SelectMany(s => regex.Split(s.ToLower().Trim()))
                .Where(s => s != string.Empty));
        }
    }
}
