namespace TagsCloud.Interfaces
{
	public interface ITextReader
	{
		Result<string[]> Read();
	}
}