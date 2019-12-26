using System;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudForm.Common
{
    public class TextReader : ITextReader
    {
        public Result<IEnumerable<string>> ReadLines(string fileName)
        {
            IEnumerable<string> lines;
            try
            {
                lines = File.ReadLines(fileName);
            }
            catch (Exception e)
            {
                return new Result<IEnumerable<string>>("Не удалось загрузить файл " + fileName+" "+e.Message, new List<string>());
            }

            return Result.Ok(lines);
        }
    }
}
