using Results;
using System.Text.RegularExpressions;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.TextHandlers;

public class TextHandler : ITextHandler
{
    private readonly IFileReader[] readers;
    private readonly TextHandlerSettings settings;
    private readonly Result<IEnumerable<string>> words;
    private readonly Result<HashSet<string>> boringWords;

    public TextHandler(IFileReader[] readers, TextHandlerSettings settings)
    {
        this.readers = readers;
        this.settings = settings;
        words = GetWords();
        boringWords = GetBoringWords();
    }

    public TextHandler(string words, string boringWords)
    {
        this.words = Result.Ok(GetWords(words));
        this.boringWords = Result.Ok(GetWords(boringWords).ToHashSet());
    }

    public Result<Dictionary<string, int>> HandleText()
    {
        var result = new Dictionary<string, int>();
        if (!words.IsSuccess)
            return Result.Fail<Dictionary<string, int>>(words.Error);
        if (!boringWords.IsSuccess)
            return Result.Fail<Dictionary<string, int>>(boringWords.Error);

        foreach (var word in words.Value)
        {
            var lowerWord = word.ToLower();
            if (result.ContainsKey(lowerWord))
                result[lowerWord]++;
            else if (!boringWords.Value.Contains(lowerWord))
                result[lowerWord] = 1;
        }

        return Result.Ok(result);
    }

    public IEnumerable<string> GetWords(string boringWords)
    {
        var pattern = new Regex(@"\b[\p{L}]+\b", RegexOptions.Compiled);

        foreach (var word in pattern.Matches(boringWords).ToHashSet())
            yield return word.Value;
    }

    public Result<IFileReader> GetReader(string path)
    {
        var reader = readers.Where(r => r.CanRead(path)).FirstOrDefault();
        return reader is null 
            ? Result.Fail<IFileReader>($"Can't read file with path {Path.GetFullPath(path)}") 
            : Result.Ok(reader);
    }

    private Result<IEnumerable<string>> GetWords()
    {
        var wordsReader = GetReader(settings.PathToText);
        if (!wordsReader.IsSuccess)
            return Result.Fail<IEnumerable<string>>(wordsReader.Error);
        var text = wordsReader.Value.ReadText(settings.PathToText);
        if (!text.IsSuccess)
            return Result.Fail<IEnumerable<string>>(text.Error);
        else
            return Result.Ok(GetWords(text.Value));
    }

    private Result<HashSet<string>> GetBoringWords()
    {
        var boringWordsReader = GetReader(settings.PathToBoringWords);
        if (!boringWordsReader.IsSuccess)
            return Result.Fail<HashSet<string>>(boringWordsReader.Error);
        var text = boringWordsReader.Value.ReadText(settings.PathToBoringWords);
        if (!text.IsSuccess)
            return Result.Fail<HashSet<string>>(text.Error);
        else
            return Result.Ok(GetWords(text.Value.ToLower()).ToHashSet());
    }
}