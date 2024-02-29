namespace TagCloud;

public interface IWordsReader
{
    Result<List<string>> Get(string path);
}