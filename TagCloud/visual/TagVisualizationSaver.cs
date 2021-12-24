using System.Drawing;

namespace TagCloud.visual
{
    public class TagVisualizationSaver : ISaver<Image>
    {
        public Result Save(Image image, string filename) =>
            Result.OfAction(() => image.Save(filename), ResultErrorType.SaveFileError);
    }
}