namespace TagCloud.visual
{
    public interface ISaver<in T>
    {
        Result Save(T obj, string filename);
    }
}