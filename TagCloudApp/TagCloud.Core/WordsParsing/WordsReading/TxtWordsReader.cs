using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagCloud.Core.Util;

namespace TagCloud.Core.WordsParsing.WordsReading
{
    public class TxtWordsReader : IWordsReader
    {
        public Regex AllowedFileExtension { get; } = new Regex(@"\.txt$", RegexOptions.IgnoreCase);

        public Result<IEnumerable<string>> ReadFrom(string path)
        {
            return Result.Of(() => ReadUnsafe(path));
        }

        private IEnumerable<string> ReadUnsafe(string path)
        {
            var res = new List<string>();
            using (var r = new StreamReader(path))
            {
                var curWord = string.Empty;
                int i;
                while ((i = r.Read()) != -1)
                {
                    var c = Convert.ToChar(i);
                    if (char.IsLetterOrDigit(c))
                    {
                        curWord = curWord + c;
                        continue;
                    }

                    if (curWord.Trim() == string.Empty) continue;
                    res.Add(curWord);
                    curWord = string.Empty;
                }

                if (curWord != string.Empty)
                    res.Add(curWord);
            }

            return res;
        }
    }
}