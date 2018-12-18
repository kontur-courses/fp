using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.DataReader;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.Filter
{
    public class BoringWordsFilter : IFilter
    {
        private readonly Result<HashSet<string>> boringWords;

        public BoringWordsFilter(IBoringWordsFilterSettings settings, IDataReader fileReader)
        {
            boringWords = fileReader.Read(settings.BoringWordsFileName)
                .Then(words => Result.Ok(new HashSet<string>(words)))
                .RefineError("Settings file with boring words could not be read");
        }

        public Result<IEnumerable<string>> FilterOut(IEnumerable<string> words)
        {
            return boringWords.Then(
                excludedWords => words.GroupBy(word => word)
                    .Where(group => !excludedWords.Contains(group.Key))
                    .SelectMany(group => group));
        }
    }
}