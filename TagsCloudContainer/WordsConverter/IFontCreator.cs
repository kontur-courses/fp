namespace TagsCloudContainer.WordsConverter
{
    public interface IFontCreator
    {
        float GetFontSize(int wordFrequency, int maxWordFrequency);
        Result<string> GetFontName();
    }
}