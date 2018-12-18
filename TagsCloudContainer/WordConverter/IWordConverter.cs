namespace TagsCloudContainer.WordConverter
{
    public interface IWordConverter
    {
        Result<string> Convert(string word);
    }
}