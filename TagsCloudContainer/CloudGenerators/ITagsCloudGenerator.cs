namespace TagsCloudContainer.CloudGenerators;

public interface ITagsCloudGenerator
{
    public Result<ITagCloud> Generate(IEnumerable<WordDetails> wordsDetails);
}