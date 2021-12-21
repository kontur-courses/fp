using System.Collections.Generic;
using TagsCloudContainerCore.StatisticMaker;

namespace TagsCloudContainerCore.InterfacesCore;

public interface ITagMaker

{
    public TagToRender MakeTag(KeyValuePair<string, int> raw, IStatisticMaker statisticMaker);
}