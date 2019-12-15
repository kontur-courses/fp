namespace TagsCloudGenerator.Interfaces
{
    public interface IGenerator
    {
        FailuresProcessing.Result<FailuresProcessing.None> Generate(string readFromPath, string saveToPath);
    }
}