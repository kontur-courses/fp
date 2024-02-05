using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public interface IWordProcessor
    {
        Result<Dictionary<string, int>> CalculateFrequencyInterestingWords(string sourceFilePath, string boringFilePath);
    }
}