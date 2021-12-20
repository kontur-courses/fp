namespace TagsCloudVisualizationDI.TextAnalyze.Normalizer
{
    public interface INormalizer
    {
        Result<string> Normalize(string stringWord);
    }
}
