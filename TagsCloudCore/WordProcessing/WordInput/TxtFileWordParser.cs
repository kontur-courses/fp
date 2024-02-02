using TagsCloudCore.Common.Enums;

namespace TagsCloudCore.WordProcessing.WordInput;

public class TxtFileWordParser : IWordProvider
{
    public Result<string[]> GetWords(string resourceLocation)
    {
        return Result.Of(() => File.ReadAllLines(resourceLocation),
            $"Failed to read from file {resourceLocation} Most likely the file path is incorrect or the file is corrupted.");
    }

    public WordProviderType Info => WordProviderType.Txt;
}