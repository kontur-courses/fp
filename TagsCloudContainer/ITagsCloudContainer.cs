namespace TagsCloudContainer;

public interface ITagsCloudContainer
{
    public Result<None> GenerateImageToFile(string inputFile, string outputFile);
}