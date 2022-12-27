namespace TagsCloud.Core.WordReaders;

public class WordReaderFromTxt : IWordReader
{
	private readonly string path;

	private WordReaderFromTxt(string path)
	{
		this.path = path;
	}

	public IEnumerable<string> ReadWords()
	{
		return File.ReadLines(path);
	}

	public static Result<WordReaderFromTxt> GetReader(string path)
	{
		if (!File.Exists(path))
			return Result.Fail<WordReaderFromTxt>($"Can't find file at path: {path}");

		return new WordReaderFromTxt(path);
	}
}