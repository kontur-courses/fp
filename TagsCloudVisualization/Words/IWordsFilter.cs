namespace TagsCloudVisualization.Words;

public interface IWordsFilter
{
    Result<Dictionary<string, int>> FilterWords(Dictionary<string, int> wordsAndCount, VisualizationOptions options);
}