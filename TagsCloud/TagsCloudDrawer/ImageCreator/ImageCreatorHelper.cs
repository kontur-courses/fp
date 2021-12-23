using System.Collections.Generic;
using System.Drawing;
using ResultMonad;
using TagsCloudDrawer.Drawer;
using TagsCloudDrawer.ImageSaveService;
using TagsCloudDrawer.ImageSettings;

namespace TagsCloudDrawer.ImageCreator
{
    public class ImageCreatorHelper
    {
        private readonly Image _image;

        public ImageCreatorHelper(Image image)
        {
            _image = image;
        }

        public Result<None> Draw(IDrawer drawer, IImageSettingsProvider settingsProvider,
            IEnumerable<Result<IDrawable>> drawables)
        {
            using var graphics = Graphics.FromImage(_image);
            graphics.Clear(settingsProvider.BackgroundColor);
            return drawer.Draw(graphics, _image.Size, drawables);
        }

        public Result<None> Save(IImageSaveService saveService, string filename)
        {
            return Result.Of(() => saveService.Save(filename, _image), "Cannot create image");
        }
    }
}