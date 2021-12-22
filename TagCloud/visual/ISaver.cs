namespace TagCloud.visual
{
    public interface ISaver<in T>
    {
        Result<None> Save(T obj, string? filename);
    }
}