namespace TagsCloudContainer.FrequencyAnalyzers
{
    public interface IAnalyzer
    {
        public static IEnumerable<(string, int)> Analyze(string text, IEnumerable<string> excludeWords = null)
        {
            return new List<(string, int)>();
        }
    }
}