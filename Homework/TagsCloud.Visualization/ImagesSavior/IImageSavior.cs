using System.Drawing;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.ImagesSavior
{
    public interface IImageSavior
    {
        Result<Image> Save(Image image);
    }
}