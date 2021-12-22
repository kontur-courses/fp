using System.Collections.Generic;
using System.Linq;
using TagsCloudContainerCore.Result;
using TagsCloudContainerCore.WordFilter;

namespace TagsCloudContainerCore.StatisticMaker;

// ReSharper disable once UnusedType.Global
public class TagStatisticMaker : IStatisticMaker
{
    private readonly IDictionary<string, int> tagsWithCount = new Dictionary<string, int>();

    private readonly IWordSelector wordSelector;

    private IList<KeyValuePair<string, int>> orderedPairs = new List<KeyValuePair<string, int>>();

    private bool isChangeTags;

    public TagStatisticMaker(IWordSelector wordSelector)
    {
        this.wordSelector = wordSelector;
    }

    public IEnumerable<KeyValuePair<string, int>> CountedTags => tagsWithCount;

    public KeyValuePair<string, int> GetLeastFrequentTag()
    {
        // ReSharper disable once InvertIf
        if (isChangeTags)
        {
            orderedPairs = tagsWithCount.OrderBy(x => x.Value).ToList();
            isChangeTags = false;
        }

        return orderedPairs[1];
    }

    public KeyValuePair<string, int> GetMostFrequentTag()
    {
        // ReSharper disable once InvertIf
        if (isChangeTags)
        {
            orderedPairs = tagsWithCount.OrderBy(x => x.Value).ToList();
            isChangeTags = false;
        }

        return orderedPairs[^1];
    }

    public Result<None> AddTagValues(IEnumerable<string> words)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        var selectWordsRes = wordSelector.SelectWords(words);

        if (!selectWordsRes.IsSuccess)
        {
            return ResultExtension.Fail<None>(selectWordsRes.Error);
        }

        foreach (var tag in selectWordsRes.Value)
        {
            AddTag(tag);
        }

        return ResultExtension.Ok();
    }

    private void AddTag(string tag)
    {
        tag = tag.ToLowerInvariant();

        isChangeTags = true;

        if (!tagsWithCount.ContainsKey(tag))
        {
            tagsWithCount[tag] = 0;
        }

        tagsWithCount[tag]++;
    }
}