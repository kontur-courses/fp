using System.Collections.Generic;
using System.Drawing;
using ResultMonad;
using TagsCloudDrawer.Drawer;
using TagsCloudDrawer.ImageSaveService;
using TagsCloudDrawer.ImageSettings;

namespace TagsCloudDrawer.ImageCreator
{
    public class ImageCreator : IImageCreator
    {
        private readonly IDrawer _drawer;
        private readonly IImageSaveService _saveService;
        private readonly IImageSettingsProvider _settingsProvider;

        public ImageCreator(IDrawer drawer, IImageSaveService saveService,
            IImageSettingsProvider settingsProvider)
        {
            _drawer = drawer;
            _saveService = saveService;
            _settingsProvider = settingsProvider;
        }

        public Result<None> Create(string filename, IEnumerable<Result<IDrawable>> drawables)
        {
            var size = _settingsProvider.ImageSize;
            using var bitmap = new Bitmap(size.Width, size.Height);
            var helper = new ImageCreatorHelper(bitmap);
            return helper
                .Draw(_drawer, _settingsProvider, drawables)
                .Then(() => helper.Save(_saveService, filename));
        }
    }
}