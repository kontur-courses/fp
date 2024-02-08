namespace TagCloud;

public interface IWordsForCloudGenerator
{
    Result<IEnumerable<WordForCloud>> Generate(Result<List<string>> words);
}