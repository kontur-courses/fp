#region

using System.Drawing;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface ITagCloudCreator
    {
        Result<Bitmap> CreateAndSaveCloudFromTextFile(string inputPath);
    }
}