using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.WordFilter;

// ReSharper disable once UnusedType.Global
public class ExcludeWordFilter : IWordSelector
{
    private readonly HashSet<string> excludedWords = new();

    public ExcludeWordFilter(IEnumerable<string> excludeWords)
    {
        excludedWords.UnionWith(excludeWords.Select(word => word.ToLowerInvariant()));
    }

    public Result<IEnumerable<string>> SelectWords(IEnumerable<string> words)
    {
        try
        {
            return ResultExtension.Ok(words.Select(word => word.ToLowerInvariant())
                .Where(word => !excludedWords.Contains(word)));
        }
        catch (ArgumentNullException e)
        {
            return ResultExtension.Fail<IEnumerable<string>>($"{e.Message}");
        }
    }
}