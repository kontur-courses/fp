using System.IO;
using TagCloudResult;

namespace TagsCloudTextProcessing.Readers
{
    public class TxtTextReader : ITextReader
    {
        private readonly string path;

        public TxtTextReader(string path)
        {
            this.path = path;
        }

        public Result<string> ReadText()
        {
            return File.Exists(path) 
                ? Result.Ok(File.ReadAllText(path)) 
                : Result.Fail<string>($"FILE {path} doesn't exist");
        }
    }
}