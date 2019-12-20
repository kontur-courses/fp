using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Word_Counting
{
    public interface IWordNormalizer
    {
        Result<string> Normalize(string word);
    }
}