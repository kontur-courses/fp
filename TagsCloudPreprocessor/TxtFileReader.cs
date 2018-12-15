using System.IO;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadFromFile(string path)
        {
            if (!File.Exists(path)) return Result.Fail<string>("File does not exist.");
            using (var sr = new StreamReader(path))
            {
                var textResult = Result.Of(() => sr.ReadToEnd());

                if (!textResult.IsSuccess) return Result.Fail<string>("Can not read input file");
                var text = textResult.GetValueOrThrow();
                return text.Length != 0 ? Result.Ok(text) : Result.Fail<string>("Empty file");
            }
        }
    }
}