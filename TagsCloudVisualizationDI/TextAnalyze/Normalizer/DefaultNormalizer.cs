namespace TagsCloudVisualizationDI.TextAnalyze.Normalizer
{
    public class DefaultNormalizer : INormalizer
    {
        public Result<string> Normalize(string str)
        {
            return Result.Of(() => str.ToLower());
        }
    }
}
