using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Filtering
{
    public class BlacklistWordsFilter : IWordsFilter
    {
        private IBoringWordsRepository BoringWordsRepository { get; }


        public BlacklistWordsFilter(IBoringWordsRepository boringWordsRepository)
        {
            BoringWordsRepository = boringWordsRepository;
        }


        public Result<ReadOnlyCollection<string>> Filter(IEnumerable<string> words)
        {
            return Result.Ok(
                new ReadOnlyCollection<string>(words.Where(x => !BoringWordsRepository.Words.Contains(x)).ToList()));
        }
    }
}