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

        var tagResults = statisticMaker
            .CountedTags
            .Select(t => tagMaker.MakeTag(t, statisticMaker))
            .ToList();

        if (tagResults.Any(res => !res.IsSuccess))
        {
            return ResultExtension.Fail<IEnumerable<TagToRender>>(
                "При получении тегов возникла ошибка\n" +
                $"{tagResults.FirstOrDefault(t => !t.IsSuccess).Error}");
        }

        return ResultExtension.Ok(tagResults.Select(t => t.GetValueOrThrow()));
    }
}