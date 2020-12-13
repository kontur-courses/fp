using System.Drawing;
using CloudLayouters;
using ResultOf;

namespace TagCloudGUIClient
{
    public class CircularLayoutFabric : IBaseCloudLayouterFabric
    {
        public string Name => "Круглое облако";

        public Result<BaseCloudLayouter> Create(Point center)
        {
            return new CircularCloudLayouter(center);
        }
    }
}