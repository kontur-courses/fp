using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.StatisticMaker;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IStatisticMaker
{
    Result<None> AddTagValues(IEnumerable<string> words);
    public Result<KeyValuePair<string, int>> GetMostFrequentTag();
    public Result<KeyValuePair<string, int>> GetLeastFrequentTag();
    IEnumerable<KeyValuePair<string, int>> CountedTags { get; }
}