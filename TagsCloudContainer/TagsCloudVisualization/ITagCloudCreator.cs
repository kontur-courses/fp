using System.Drawing;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization
{
    public interface ITagCloudCreator
    {
        public Result<Bitmap> CreateFromFile(string filepath);
    }
}