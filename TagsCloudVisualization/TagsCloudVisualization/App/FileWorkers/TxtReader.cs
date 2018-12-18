using System;
using System.IO;

namespace TagsCloudVisualization
{
    public class TxtReader : IFileReader
    {
        public Result<string> Read(string fileName)
        {
            try
            {
                var file = File.ReadAllText(fileName);
                return file;
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }
        }
    }
}
