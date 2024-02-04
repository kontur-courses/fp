using Autofac.Features.AttributeFilters;
using TagsCloud.ConsoleCommands;
using TagsCloud.Result;
using TagsCloud.WordValidators;

namespace TagsCloud.WordsProviders;

public class WordsProvider : IWordsProvider
{
    private readonly IWordValidator validator;
    private readonly string filename;

    public WordsProvider(IWordValidator validator, Options options)
    {
        this.validator = validator;
        this.filename = options.InputFile;
    }

    public Result<Dictionary<string, int>> GetWords()
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(filename);
        var dict= File.ReadAllText(filename)
            .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)
            .GroupBy(word => word.ToLower())
            .Where(g => validator.IsWordValid(g.Key).GetValueOrThrow())
            .OrderByDescending(g2 => g2.Count())
            .ToDictionary(group => group.Key, group => group.Count());
        return Result.Result.Ok(dict);
    }
}