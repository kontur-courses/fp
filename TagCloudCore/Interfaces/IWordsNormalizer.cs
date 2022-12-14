namespace TagCloudCore.Interfaces;

public interface IWordsNormalizer
{
    public IEnumerable<string> GetWordsOriginalForm(string sourceText);
}