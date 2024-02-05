using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloud.App.Settings;

namespace TagsCloud.App.Infrastructure;

public class PictureBoxImageHolder : PictureBox, IImageHolder
{
    public Result<Size> GetImageSize()
    {
        return FailIfNotInitialized().Then(_ => Image.Size);
    }

    public Result<Graphics> StartDrawing()
    {
        return FailIfNotInitialized().Then(_ => Graphics.FromImage(Image));
    }

    public void UpdateUi()
    {
        Refresh();
        Application.DoEvents();
    }

    public Result<None> RecreateImage(ImageSettings imageSettings)
    {
        if (imageSettings.Height <= 0 || imageSettings.Width <= 0)
            return Result.Fail<None>("размеры изображения не могут быть отрицательными или равными нулю");

        Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        return Result.Ok();
    }

    public Result<None> SaveImage(string fileName)
    {
        return FailIfNotInitialized().Then(_ => Image.Save(fileName));
    }

    private Result<None> FailIfNotInitialized()
    {
        return Image == null
            ? Result.Fail<None>("Call PictureBoxImageHolder.RecreateImage before other method call!")
            : Result.Ok();
    }
}