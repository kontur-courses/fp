using System.Collections.Generic;
using System.Linq;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.Result;
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

    public Result<IEnumerable<TagToRender>> GetTagsToRender(IEnumerable<string> tags)
    {
        var statisticResult = statisticMaker.AddTagValues(tags);

        if (!statisticResult.IsSuccess)
        {
            return ResultExtension.Fail<IEnumerable<TagToRender>>(statisticResult.Error);
        }

        return ResultExtension.Ok(statisticMaker
            .CountedTags
            .Select(t => tagMaker.MakeTag(t, statisticMaker)));
    }
}