using System.Drawing;
using ResultOf;

namespace TagCloud.Common.Saver;

public interface ICloudSaver
{
    public Result<None> SaveCloud(Bitmap bmp);
}