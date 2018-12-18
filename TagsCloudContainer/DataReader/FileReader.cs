using System.Collections.Generic;
using System.IO;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.DataReader
{
    public class FileReader : IDataReader
    {
        public Result<IEnumerable<string>> Read(string filename)
        {
            return Result.Of(() => File.ReadLines(filename), $"File {filename} not found or not available");
        }
    }
}