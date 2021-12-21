using System.Collections.Generic;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.StatisticMaker;

namespace TagsCloudContainerCore.TagCloudMaker;

// ReSharper disable once UnusedType.Global
public class TagCloudMaker : ITagCloudMaker
{
    private readonly IStatisticMaker statisticMaker;
    private readonly ITagMaker tagMaker;


    public TagCloudMaker(
        IStatisticMaker statisticMaker,
        ITagMaker tagMaker)
    {
        this.statisticMaker = statisticMaker;
        this.tagMaker = tagMaker;
    }

    public IEnumerable<TagToRender> GetTagsToRender(IEnumerable<string> tags)
    {
        statisticMaker.AddTagValues(tags);

        foreach (var rawTag in statisticMaker.CountedTags)
        {
            var tag = tagMaker.MakeTag(rawTag, statisticMaker);

            yield return tag;
        }
    }
}