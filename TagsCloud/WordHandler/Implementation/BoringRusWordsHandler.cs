namespace TagCloud.WordHandler.Implementation;

public class BoringRusWordsHandler : IWordHandler
{
    private readonly string[] prepositions = {
        "в", "без", "до", "для", "за", "через", "над", "по", "из", "у", "около", "под", "о", "про", "на", "к", "перед",
        "при", "с", "между"
    };

    private readonly string[] pronouns =
    {
        "я", "он", "ты", "кто-то",
        "что-то", "что-либо", "кое-что",
        "любой", "иной", "какой-то",
        "столько",
    };

    public IEnumerable<string> ProcessWords(IEnumerable<string> words) =>
        words.Where(word => ProcessWord(word) is not null);

    public string? ProcessWord(string word) => prepositions.Contains(word) || pronouns.Contains(word) ? null : word;
}