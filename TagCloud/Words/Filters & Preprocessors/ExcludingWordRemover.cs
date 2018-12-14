using System.Collections.Generic;
using System.Linq;

namespace TagCloud.Words
{
    public class ExcludingWordsRemover : IWordFilter
    {
        private readonly IExcludingRepository excludingWordsRepository;

        public ExcludingWordsRemover(IExcludingRepository excludingWordsRepository)
        {
            this.excludingWordsRepository = excludingWordsRepository;
        }

        public Result<IEnumerable<string>> Filter(IEnumerable<string> words)
        {
            return words.Where(w => !excludingWordsRepository.Contains(w)).AsResult();
        }
    }
}