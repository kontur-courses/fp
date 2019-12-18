using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.WordProcessing.Converting
{
    public class ToLowerWordConverter : IWordConverter
    {
        public Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words)
        {
            return words.Select(w => w.ToLower()).AsResult();
        }
    }
}