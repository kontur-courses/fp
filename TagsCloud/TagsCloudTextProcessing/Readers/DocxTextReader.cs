using System.IO;
using TagCloudResult;
using Xceed.Words.NET;

namespace TagsCloudTextProcessing.Readers
{
    public class DocxTextReader : ITextReader
    {
        private readonly string path;
        public DocxTextReader(string path)
        {
            this.path = path;
        }
        public Result<string> ReadText()
        {
            if(!File.Exists(path))
                return Result.Fail<string>($"FILE {path} doesn't exist");
            
            using (var document = DocX.Load(path))
                return Result.Ok(document.Text);
        }
    }
}