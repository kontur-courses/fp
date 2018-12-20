using System.IO;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var text= sr.ReadToEnd();

                return text.Length != 0 ? Result.Ok(text) : Result.Fail<string>("Empty file");
            }
        }
    }
}