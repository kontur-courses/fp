using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ITagCloudCreator
    {
        Result<Bitmap> CreateTagsCloudBitmapFromTextFile(string inputPath);
    }
}