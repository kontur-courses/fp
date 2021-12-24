using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace TagCloud.selectors
{
    public class WordFilter : IFilter<string>
    {
        private readonly List<IChecker<string>> checkers;

        public WordFilter([NotNull] List<IChecker<string>> checkers)
        {
            this.checkers = checkers;
        }

        public Result<IEnumerable<string>> Filter(IEnumerable<string> source) =>
            Result.Of(() => checkers.Count == 0
                    ? source
                    : source.Where(word => checkers.Any(checker => checker.IsValid(word).IsSuccess)),
                ResultErrorType.FilterError);
    }
}