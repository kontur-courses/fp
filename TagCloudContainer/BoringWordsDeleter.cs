using System.Collections.Generic;
using System.Linq;
using TagCloudContainer.TaskResult;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    public static class BoringWordsDeleter
    {
        public static Result<IEnumerable<string>> DeleteBoringWords(IEnumerable<string> words)
        {
            return !words.Any(str => str.Length > 0)
                ? Result.OnFail<IEnumerable<string>>("Empty file")
                : Result.OnSuccess<IEnumerable<string>>(words.Where(word => word.Length > 3));
        }
    }
}