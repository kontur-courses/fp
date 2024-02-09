namespace TagCloudResult.TextProcessing
{
    public interface ITextProcessor
    {
        public Result<Dictionary<string, int>> GetWordsFrequency();
    }
}
