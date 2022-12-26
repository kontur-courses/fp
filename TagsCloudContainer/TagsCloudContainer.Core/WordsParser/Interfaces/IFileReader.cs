namespace TagsCloudContainer.Core.WordsParser.Interfaces
{
    public interface IFileReader
    {
        public IEnumerable<string> ReadWords();
    }
}
