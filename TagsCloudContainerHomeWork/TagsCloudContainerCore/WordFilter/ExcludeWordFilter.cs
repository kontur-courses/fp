using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainerCore.WordFilter;

// ReSharper disable once UnusedType.Global
public class ExcludeWordFilter : IWordSelector
{
    private readonly HashSet<string> excludedWords = new();

    public ExcludeWordFilter(IEnumerable<string> excludeWords)
    {
        excludedWords.UnionWith(excludeWords.Select(word => word.ToLowerInvariant()));
    }

    public IEnumerable<string> SelectWords(IEnumerable<string> words)
        => words.Select(word => word.ToLowerInvariant())
            .Where(word => !excludedWords.Contains(word));
}