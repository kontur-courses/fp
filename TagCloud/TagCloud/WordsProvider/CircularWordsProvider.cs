using System.Collections.Generic;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class CircularWordsProvider : IWordsProvider
    {
        public const int WordsCount = 100;
        public List<string> words = new List<string> {"word", "another", "123", "VeryVeryLongWord", "word"};

        public Result<IEnumerable<string>> GetWords()
        {
            IEnumerable<string> Words()
            {
                var i = 0;
                while (i < WordsCount)
                {
                    yield return words[i % words.Count];
                    i++;
                }
            }

            return Result.Ok(Words());
        }
    }
}