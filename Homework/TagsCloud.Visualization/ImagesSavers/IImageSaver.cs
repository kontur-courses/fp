using System.Drawing;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.ImagesSavers
{
    public interface IImageSaver
    {
        Result<Image> Save(Image image);
    }
}