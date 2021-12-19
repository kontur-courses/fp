namespace TagsCloudContainer.Interfaces;

public interface ISpiral
{
    string Name { get; }

    Point GetNext();

    void Reset();
}
