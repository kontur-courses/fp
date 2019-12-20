using System.Collections.Generic;
using System.IO;
using ResultOf;

namespace TagCloud.TextReading
{
    public class TxtTextReader : ITextReader
    {
        public Result<IEnumerable<string>> ReadWords(FileInfo file)
        {
            try
            {
                return Result.Ok(File.ReadLines(file.FullName));
            }
            catch (IOException e)
            {
                return Result
                    .Fail<IEnumerable<string>>(e.Message)
                    .RefineError($"File {file.FullName} is in use");
            }
            
        }

        public string Extension => ".txt";
    }
}
