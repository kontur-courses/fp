using System.Drawing.Imaging;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Savers;

public interface ITagCloudSaver
{
    Result<None> SaveTagCloud(string inputPath, string outputPath, ImageFormat format);
}