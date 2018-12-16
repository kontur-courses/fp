using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloud.WordPrework;

namespace TagsCloud.Tests
{
    public class SimpleWordGetter: IWordsGetter
    {
        public readonly IEnumerable<string> Words;

        public SimpleWordGetter(IEnumerable<string> words)
        {
            Words = words;
        }

        public IEnumerable<Result<string>> GetWords(params char[] delimiters)
        {
            foreach (var word in Words)
            {
                yield return word;
            }
        }
    }
}
