using System.Linq;
using TagsCloudVisualization.WordProcessors.WordProcessingSettings;

namespace TagsCloudVisualization.WordProcessors
{
    public class WordProcessor : IWordProcessor 
    {
        public IProcessingSettings Settings { get; }

        public bool WordIsAllowed(string word)
        {
            return word.Length >= Settings.MinWordLength &&
                   word.Length <= Settings.MaxWordLength &&
                   !Settings.ExcludedWords.Contains(word);
        }

        public Result<string[]> Process(string[] words)
        {
            var processResult = Result.Of(() => words.Where(WordIsAllowed)
                .Select(word => word.ToLower()).ToArray());

            if (processResult.IsSuccess)
                return processResult;

            return Result.Fail<string[]>(processResult.Error);
        }

        public WordProcessor(IProcessingSettings settings)
        {
            Settings = settings;
        }
    }
}
