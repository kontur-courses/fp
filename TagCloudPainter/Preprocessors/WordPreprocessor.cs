using TagCloudPainter.Common;
using TagCloudPainter.Lemmaizers;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Preprocessors;

public class WordPreprocessor : IWordPreprocessor
{
    private readonly ILemmaizer _lemmaizer;
    private readonly ParseSettings parseSettings;

    public WordPreprocessor(IParseSettingsProvider parseSettingsProvider, ILemmaizer lemmaizer)
    {
        _lemmaizer = lemmaizer;
        parseSettings = parseSettingsProvider.ParseSettings;
    }

    public Result<Dictionary<string, int>> GetWordsCountDictionary(IEnumerable<string> words)
    {
        if (words == null || words.Count() == 0)
            return Result.Fail<Dictionary<string, int>>("words is null or Empty");

        var wordsCount = new Dictionary<string, int>();
        foreach (var word in words)
        {
            var skip = IsSkiped(word);
            if (!skip.IsSuccess)
                return Result.Fail<Dictionary<string, int>>($"{skip.Error}");

            if (IsSkiped(word).Value)
                continue;

            var lemma = _lemmaizer.GetLemma(word);
            if (!lemma.IsSuccess)
                return Result.Fail<Dictionary<string, int>>($"{lemma.Error}");

            var key = lemma.Value.ToLower();
            if (wordsCount.ContainsKey(key))
                wordsCount[key]++;
            else
                wordsCount[key] = 1;
        }

        return wordsCount;
    }

    private Result<bool> IsSkiped(string word)
    {
        if (parseSettings.IgnoredWords.Count > 0 && parseSettings.IgnoredWords.Contains(word))
            return true;

        var morpheme = _lemmaizer.GetMorph(word);

        if (!morpheme.IsSuccess)
            return Result.Fail<bool>($"{morpheme.Error}");

        if (parseSettings.IgnoredMorphemes.Contains(morpheme.Value))
            return true;

        return false;
    }
}