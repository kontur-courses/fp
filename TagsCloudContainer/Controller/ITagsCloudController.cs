using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.Controller
{
    public interface ITagsCloudController
    {
        Result<None> Save();
    }
}