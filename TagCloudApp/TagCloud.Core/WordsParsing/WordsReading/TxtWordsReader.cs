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

        public Result<IEnumerable<string>> ReadFrom(Stream stream)
        {
            return Result.Of(() => ReadUnsafeFrom(stream));
        }

        private IEnumerable<string> ReadUnsafeFrom(Stream stream)
        {
            var res = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                var curWord = string.Empty;
                int i;
                while ((i = reader.Read()) != -1)
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