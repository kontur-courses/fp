namespace TagsCloudVisualizationDI.TextAnalyze.Normalizer
{
    public class DefaultNormalizer : INormalizer
    {
        public string Normalize(string str)
        {
            return str.ToLower();
        }
    }
}
