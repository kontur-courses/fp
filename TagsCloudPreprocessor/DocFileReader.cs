using ResultOfTask;
using Xceed.Words.NET;

namespace TagsCloudPreprocessor
{
    public class DocFileReader : IFileReader
    {
        public Result<string> ReadFromFile(string path)
        {
            var text = DocX.Load(path).Text;
            return text.Length != 0 ? Result.Ok(text) : Result.Fail<string>("Empty file");
        }
    }
}