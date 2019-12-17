using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.PathFinders;

namespace TagsCloudVisualization.Visualization
{
    public static class ImageSaver
    {
        public static Result<string> SaveImageToDefaultDirectory(string name, Bitmap image, ImageFormat format)
        {
            var saveResult = Result.Of(() => PathFinder.GetImagesPath(name, format))
                .Then(path => image.Save(path, format));
            
            return saveResult.IsSuccess
                ? PathFinder.GetImagesPath(name, format).AsResult()
                : Result.Fail<string>("Failed to save file");
        }

        public static Result<string> SaveImage(string path, Bitmap image, ImageFormat format)
        {
            var saveResult = Result.OfAction(() => image.Save(path, format));
            
            return saveResult.IsSuccess
                ? path.AsResult()
                : Result.Fail<string>("Failed to save file");
        }
    }
}