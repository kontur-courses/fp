namespace TagsCloudContainer
{
    public interface ISource
    {
        Result<string[]> GetWords();
    }
}
