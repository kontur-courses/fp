using System.Diagnostics.CodeAnalysis;
using DeepMorphy;
using DeepMorphy.Model;
using TagsCloud.App.Settings;

namespace TagsCloud.WordAnalyzer;

public class WordAnalyzer
{
    private readonly string Link = "https://github.com/lepeap/DeepMorphy/blob/master/README.md";
    private readonly WordAnalyzerSettings Settings;

    public WordAnalyzer(WordAnalyzerSettings settings)
    {
        Settings = settings;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.String; size: 127MB")]
    private Result<IEnumerable<string>> GetFilteredWords(IEnumerable<string?> words)
    {
        var morphAnalyzer = new MorphAnalyzer(true);
        var morphInfos = Result.Of(() => morphAnalyzer.Parse(words));

        if (!morphInfos.IsSuccess)
        {
            var g = new Uri(Link);
            return Result.Fail<IEnumerable<string>>(
                $"Ошибка внутренней библиотеки Deep Morphy: {morphInfos.Error} +\n\n\n" +
                $"Посмотри документацию по ссылке: {Link}");
        }

        return Result.Ok(morphInfos.Value
            .Where(IsCurrentWordForAnalysis)
            .Select(info => info.BestTag.HasLemma ? info.BestTag.Lemma : info.Text));
    }

    private bool IsCurrentWordForAnalysis(MorphInfo morphInfo)
    {
        var excludedSpeeches = WordAnalyzerHelper.GetConvertedSpeeches(Settings.ExcludedSpeeches);
        var selectedSpeeches = WordAnalyzerHelper.GetConvertedSpeeches(Settings.SelectedSpeeches);

        return !Settings.BoringWords.Any(item =>
                   item.Equals(morphInfo.BestTag.Lemma, StringComparison.OrdinalIgnoreCase)) &&
               !excludedSpeeches.Contains(morphInfo.BestTag["чр"]) &&
               (selectedSpeeches.Count == 0 ||
                selectedSpeeches.Contains(morphInfo.BestTag["чр"]));
    }

    public Result<IEnumerable<WordInfo>> GetFrequencyList(IEnumerable<string?> words)
    {
        var parsedWords = new Dictionary<string, int>();
        foreach (var word in GetFilteredWords(words).GetValueOrThrow())
        {
            parsedWords.TryAdd(word, 0);
            parsedWords[word]++;
        }

        return Result.Ok<IEnumerable<WordInfo>>(parsedWords.Select(x => new WordInfo(x.Key, x.Value))
            .OrderByDescending(info => info.Count)
            .ThenBy(info => info.Word));
    }
}