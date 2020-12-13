using System.Drawing;
using CloudLayouters;
using ResultOf;

namespace TagCloudGUIClient
{
    public class RectangleLayouterFabric : IBaseCloudLayouterFabric
    {
        public string Name => "Квадратное облако";


        public Result<BaseCloudLayouter> Create(Point center)
        {
            return new RectangleLayouter(center);
        }
    }
}