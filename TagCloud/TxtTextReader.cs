using System;
using System.IO;
using ResultOf;

namespace TagCloud
{
    public class TxtTextReader : ITextReader
    {
        public Result<string> TryReadText(string fileName)
        {
            return Result.Of(() =>
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    
                        var fileBytes = new byte[fileStream.Length];
                        fileStream.Read(fileBytes, 0, fileBytes.Length);
                        return System.Text.Encoding.Default.GetString(fileBytes);
                    
                }
            }, "File not found");
        }
    }
}