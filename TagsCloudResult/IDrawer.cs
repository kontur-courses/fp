namespace TagsCloudResult
{
    public interface IDrawer<T>
    {
        Result<None> DrawItems(string resultFilePath = null);
    }
}
