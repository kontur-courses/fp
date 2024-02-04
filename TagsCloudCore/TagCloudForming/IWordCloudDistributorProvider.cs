using TagsCloudCore.Common;

namespace TagsCloudCore.TagCloudForming;

public interface IWordCloudDistributorProvider
{
    public Result<IReadOnlyDictionary<string, WordData>> DistributeWords();
}