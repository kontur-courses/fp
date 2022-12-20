namespace TagsCloudVisualization.MorphAnalyzer;

public interface IMorphAnalyzer
{
    Result<Dictionary<string, WordMorphInfo>> GetWordsMorphInfo(IEnumerable<string> words);
}