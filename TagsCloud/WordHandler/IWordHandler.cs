namespace TagCloud.WordHandler;

public interface IWordHandler
{
    public IEnumerable<string> ProcessWords(IEnumerable<string> words);
    public string? ProcessWord(string word);
}