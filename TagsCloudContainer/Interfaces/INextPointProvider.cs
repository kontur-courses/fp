using System.Drawing;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.Interfaces
{
    public interface INextPointProvider
    {
        Result<Point> GetNextPoint();
    }
}
