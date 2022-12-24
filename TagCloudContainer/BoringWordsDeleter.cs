using System.Collections.Generic;
using System.Linq;
using TagCloudContainer.Result;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    public static class BoringWordsDeleter
    {
        public static Result<IEnumerable<string>> DeleteBoringWords(IEnumerable<string> words)
        {
            if (!words.Any())
                return new Result<IEnumerable<string>>("Empty file");
            return new Result<IEnumerable<string>>(null, words.Where(word => word.Length > 3));
        }
    }
}