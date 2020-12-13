using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    public interface IColorSelectorFabric
    {
        string Name { get; }
        Result<IColorSelector> Create();
    }
}