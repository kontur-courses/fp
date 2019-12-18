using System;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloud.ImageSavers;

namespace TagsCloud
{
    public class ImageSaveHelper
    {
        private readonly IImageSaver[] imageSavers;

        public ImageSaveHelper(IImageSaver[] imageSavers)
        {
            this.imageSavers = imageSavers;
        }

        public Result<None> SaveTo(Image image, string filename)
        {
            return Result.Of(() => Path.GetExtension(filename))
                .Then(fileExtension => imageSavers.First(p => p.FileExtensions.Any(ext => ext == fileExtension)))
                .ReplaceError(msg => $"Can't select file saver for this file format ({filename})")
                .Then(imageSaver => imageSaver.Save(image, filename))
                .RefineError($"Can't save image to file '{filename}'");
        }
    }
}
