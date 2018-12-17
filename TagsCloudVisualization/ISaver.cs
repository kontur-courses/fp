using ResultOf;

namespace TagsCloudVisualization
{
    public interface ISaver<T>
    {
        Result<None> Save(string filename);
    }
}
