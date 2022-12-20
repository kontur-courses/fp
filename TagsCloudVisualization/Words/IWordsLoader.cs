namespace TagsCloudVisualization.Words;

public interface IWordsLoader
{
    Result<IEnumerable<Word>> LoadWords(VisualizationOptions options);
}