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
                var textResult = Result.Of(() => sr.ReadToEnd());

                var text = textResult.GetValueOrThrow();
                return text.Length != 0 ? Result.Ok(text) : Result.Fail<string>("Empty file");
            }
        }
    }
}