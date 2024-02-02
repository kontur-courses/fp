namespace TagsCloudCore.WordProcessing.WordGrouping;

public interface IProcessedWordProvider
{
    public Result<IReadOnlyDictionary<string, int>> ProcessedWords { get; }
}