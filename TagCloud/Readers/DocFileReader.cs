using System.Linq;
using Xceed.Words.NET;

namespace TagCloud.Readers
{
    public class DocFileReader : IFileReader
    {
        public Result<string[]> ReadFile(string filename)
        {
            var doc = Result.Of(() => DocX.Load(filename));
            return doc.IsSuccess 
                ? doc.Value.Paragraphs.Select(p => p.Text).ToArray()
                : Result.Fail<string[]>($"File {filename} not found." +
                                        $" Please check that file exists");
        }
    }
}
