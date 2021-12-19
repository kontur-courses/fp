namespace TagsCloudContainer.Spirals;

public interface ISpiral
{
    string Name { get; }

    Point GetNext();

    void Reset();
}
