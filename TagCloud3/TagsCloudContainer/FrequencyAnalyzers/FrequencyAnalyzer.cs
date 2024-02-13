using ResultOf;
using TagsCloudContainer.SettingsClasses;

namespace TagsCloudContainer.FrequencyAnalyzers
{
    public class FrequencyAnalyzer : IAnalyzer
    {
        public static Result<IEnumerable<(string, int)>> Analyze(string text)
        {
            string excludedWordsPath = SettingsStorage.AppSettings.FilterFile;

            var wordFrequency = new Dictionary<string, int>();

            var preprocessor = new TextPreprocessing(excludedWordsPath).Preprocess(text);

            if (!preprocessor.IsSuccess)
            {
                return Result<IEnumerable<(string, int)>>.Fail(preprocessor.Error);
            }

            foreach (var word in preprocessor.GetValueOrDefault())
            {
                if (wordFrequency.ContainsKey(word.ToLower()))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency.Add(word, 1);
                }
            }

            return Result<IEnumerable<(string, int)>>.Ok(wordFrequency.Select(p => (p.Key, p.Value)));
        }
    }
}