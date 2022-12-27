namespace TagCloud.WordHandler.Implementation;

public class RecurringWordsHandler : IWordHandler
{
    public Dictionary<string, int> WordCount = new();

    public IEnumerable<string> ProcessWords(IEnumerable<string> words) =>
        words.Where(word => ProcessWord(word) is not null);

    public string? ProcessWord(string word)
    {
        if (WordCount.ContainsKey(word))
        {
            WordCount[word]++;
            return null;
        }
        
        WordCount.Add(word, 1);
        return word;
    }

    public int GetMostFrequentPair => WordCount.Max(item => item.Value);
}