namespace TagsCloudContainer.Readers
{
    public interface IReader
    {
        Result<string[]> ReadAllLines();
    }
}
