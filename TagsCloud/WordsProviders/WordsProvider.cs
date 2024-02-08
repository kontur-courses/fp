using TagsCloud.Options;
using TagsCloud.WordValidators;

namespace TagsCloud.WordsProviders;

public class WordsProvider : IWordsProvider
{
    private readonly IWordValidator _validator;
    private readonly string _filename;

    public WordsProvider(IWordValidator validator, LayouterOptions options)
    {
        _validator = validator;
        _filename = options.InputFile;
    }

    public Result<Dictionary<string, int>> GetWords()
    {
        if (!File.Exists(_filename))
            return Result.Fail<Dictionary<string, int>>($"No such file or directory {_filename}");
        
        var text = File.ReadAllText(_filename);

        if (string.IsNullOrEmpty(text))
        {
            return Result.Fail<Dictionary<string, int>>($"No such words in file {_filename}");
        }
        
        var dict= text
            .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)
            .GroupBy(word => word.ToLower())
            .Where(g => _validator.IsWordValid(g.Key).GetValueOrThrow())
            .OrderByDescending(g2 => g2.Count())
            .ToDictionary(group => group.Key, group => group.Count());
        return dict;
    }
}