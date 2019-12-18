using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class DefaultWordsCounter : IWordsCounter
    {
        public DefaultWordsCounter()
        {
        }

        public Result<Dictionary<string, int>> CountWords(IEnumerable<string> arr)
        {
            if (arr.ToArray().Length == 0)
                return Result.Fail<Dictionary<string, int>>("Count of words should be > 0");
            var res = new Dictionary<string, int>();
            foreach (var str in arr)
            {
                if (res.ContainsKey(str))
                    res[str] += 1;
                else res[str] = 1;
            }

            return res;
        }
    }
}