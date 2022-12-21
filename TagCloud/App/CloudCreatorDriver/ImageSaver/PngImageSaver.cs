﻿using System.Drawing;
using System.Runtime.InteropServices;

namespace TagCloud.App.CloudCreatorDriver.ImageSaver;

public class PngImageSaver : IImageSaver
{
    public Result<None> SaveImage(Bitmap image, string fullFileName)
    {
        return Result.OfAction(() => image.Save(fullFileName));
    }
}