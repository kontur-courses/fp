using ResultOf;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface ITagPacker : IService
{
    Result<IEnumerable<ITag>> GetTags();
}