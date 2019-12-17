using ResultOF;
using Xceed.Words.NET;

namespace TagCloud
{
    public class DocReader : IReader
    {
        public Result<string> ReadAllText(string pathToFile)
        {
            using (var document = DocX.Load(pathToFile))
                return document.Text;
        }
    }
}
