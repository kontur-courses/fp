using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    public class RandomFromKnownColorsSelectorFabric : IColorSelectorFabric
    {
        public string Name => "Случайный из известных цветов";

        public Result<IColorSelector> Create()
        {
            return new RandomFromColorsColorSelector();
        }
    }
}