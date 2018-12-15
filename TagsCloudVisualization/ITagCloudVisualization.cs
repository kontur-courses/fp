using System.Collections.Generic;
using System.Drawing;
using ResultOfTask;

namespace TagsCloudVisualization
{
    public interface ITagCloudVisualization
    {
        void SaveRectanglesCloud(
            string bitmapName,
            string directory,
            List<Rectangle> rectangles,
            Point center);

        Result<None> SaveTagCloud(
            string bitmapName,
            string directory,
            Result<Dictionary<string, int>> words);
    }
}