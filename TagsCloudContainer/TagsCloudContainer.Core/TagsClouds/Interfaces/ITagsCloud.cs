using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.TagsClouds.Interfaces
{
    public interface ITagsCloud
    {
        public Result<None> CreateTagCloud();

        public Result<None> SaveTagCloud();
    }
}
