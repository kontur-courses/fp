namespace TagCloud;

public interface IWordsNormalizer
{
    Result<List<string>> NormalizeWords(Result<List<string>> words, Result<HashSet<string>> boringWords);
}