using ResultOF;
using System.Linq;

namespace TagCloud
{
    public class DefaultExtractor : IExtractor
    {
        public Result<string[]> ExtractWords(string text)
        {
            if (text == null)
                return Result.Fail<string[]>("Text cannot be null");
            var words = text.Split('\r', '\n');
            return words.Where(word => word != "").ToArray();
        }
    }
}
