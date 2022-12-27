namespace TagCloud.WordHandler.Implementation;

public class LowerCaseHandler : IWordHandler
{
    public IEnumerable<string> ProcessWords(IEnumerable<string> words) => words.Select(ProcessWord);

    public string ProcessWord(string word) => word.ToLower();
}