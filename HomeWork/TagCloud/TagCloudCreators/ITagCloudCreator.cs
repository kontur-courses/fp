namespace TagCloud.TagCloudCreators
{
    public interface ITagCloudCreator
    {
        public Result<ITagCloud> GenerateTagCloud();
    }
}
