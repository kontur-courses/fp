using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    public class FullRandomColorSelectorFabric : IColorSelectorFabric
    {
        public string Name => "Случайный цвет";

        public Result<IColorSelector> Create()
        {
            return new FullRandomColorSelector();
        }
    }
}