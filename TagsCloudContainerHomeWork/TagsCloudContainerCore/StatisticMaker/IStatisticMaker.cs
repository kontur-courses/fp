using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TagsCloudContainerCore.StatisticMaker;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IStatisticMaker
{
    void AddTagValues(IEnumerable<string> words);
    public KeyValuePair<string, int> GetMostFrequentTag();
    public KeyValuePair<string, int> GetLeastFrequentTag();
    IEnumerable<KeyValuePair<string, int>> CountedTags { get; }
}