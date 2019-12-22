using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagsCloud.ErrorHandler;

namespace TagsCloud.WordPreprocessing
{
    public class FileReader : IWordGetter
    {
        public readonly FileInfo FileName;

        public readonly Encoding Encoding;
        public readonly Regex Regex = new Regex(@"^\s*$", RegexOptions.Compiled);

        public FileReader(FileInfo fileName, Encoding encoding = null)
        {
            FileName = fileName;
            Encoding = encoding ?? Encoding.Default;
        }

        public Result<IEnumerable<string>> GetWords(params char[] delimiters)
        {
            if (!FileName.Exists)
            {
                return Result.Fail<IEnumerable<string>>($"File '{FileName}' not found");
            }
            delimiters = delimiters.ToList().Append(' ').ToArray();
            using (var sr = new StreamReader(FileName.FullName, Encoding))
            {
                return Result.Ok<IEnumerable<string>>(sr.ReadToEnd().Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                    .Where(w => !Regex.IsMatch(w)));
            }
        }
    }
}