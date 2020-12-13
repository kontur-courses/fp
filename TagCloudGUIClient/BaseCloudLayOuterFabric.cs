using System.Drawing;
using CloudLayouters;
using ResultOf;

namespace TagCloudGUIClient
{
    public interface IBaseCloudLayouterFabric
    {
        string Name { get; }
        Result<BaseCloudLayouter> Create(Point center);
    }
}