using System.Collections.Generic;
using TagsCloudContainerCore.Result;
using TagsCloudContainerCore.StatisticMaker;

namespace TagsCloudContainerCore.InterfacesCore;

public interface ITagMaker

{
    public Result<TagToRender> MakeTag(KeyValuePair<string, int> raw, IStatisticMaker statisticMaker);
}