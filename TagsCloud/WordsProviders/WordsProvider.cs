using TagsCloud.ConsoleCommands;
using TagsCloud.WordValidators;

namespace TagsCloud.WordsProviders;

public class WordsProvider : IWordsProvider
{
    private readonly IWordValidator _validator;
    private readonly string _filename;

    public WordsProvider(IWordValidator validator, Options options)
    {
        this._validator = validator;
        this._filename = options.InputFile;
    }

    public Result<Dictionary<string, int>> GetWords()
    {
        if (!File.Exists(_filename))
            return Result.Fail<Dictionary<string, int>>($"No such file or directory {_filename}");
        
        var dict= File.ReadAllText(_filename)
            .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)
            .GroupBy(word => word.ToLower())
            .Where(g => _validator.IsWordValid(g.Key).GetValueOrThrow())
            .OrderByDescending(g2 => g2.Count())
            .ToDictionary(group => group.Key, group => group.Count());
        return dict;
    }
}