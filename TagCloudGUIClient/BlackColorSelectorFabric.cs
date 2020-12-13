using System.Drawing;
using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    public class BlackColorSelectorFabric : IColorSelectorFabric
    {
        public string Name => "Черный";

        public Result<IColorSelector> Create()
        {
            return new SingleColorSelector(Color.Black);
        }
    }
}