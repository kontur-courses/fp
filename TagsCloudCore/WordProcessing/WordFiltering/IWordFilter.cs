namespace TagsCloudCore.WordProcessing.WordFiltering;

public interface IWordFilter
{
    public Result<string[]> FilterWords(string[] words);
}