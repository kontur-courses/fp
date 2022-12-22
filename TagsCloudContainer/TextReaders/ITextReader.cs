namespace TagsCloudContainer.TextReaders
{
    public interface ITextReader
    {
        public Result<string> GetTextFromFile(string path);
    }
}