namespace TagsCloudGenerator.Interfaces
{
    public interface IWordsParser : IFactorial
    {
        FailuresProcessing.Result<string[]> ParseFromFile(string filePath);
    }
}