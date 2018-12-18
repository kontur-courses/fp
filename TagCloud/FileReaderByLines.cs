using System.Collections.Generic;
using System.IO;
using System.Text;
using TagCloud.Interfaces;
using TagCloud.Result;

namespace TagCloud
{
    public class FileReaderByLines : IFileReader
    {
        public Result<IEnumerable<string>> Read(string path)
        {
            var result = new List<string>();
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                while (!sr.EndOfStream) result.Add(sr.ReadLine());
            }

            return result;
        }
    }
}