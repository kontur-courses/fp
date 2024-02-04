namespace TagsCloudVisualization;

public interface IInterestingWordsParser
{
    Result<IEnumerable<string>> GetInterestingWords(string path);
}