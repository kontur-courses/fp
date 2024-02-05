using System.Drawing.Imaging;
using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.WFApp.Common;

public class PictureBoxImageHolder : PictureBox, IImageHolder
{
    public Result<Size> GetImageSize()
    {
        return ValidateImageCreation()
            .Then(x => x.Size);
    }

    public Result<Graphics> StartDrawing()
    {
        return ValidateImageCreation()
            .Then(Graphics.FromImage);
    }

    private Result<Image> ValidateImageCreation()
    {
        return Result.Validate(Image, x => x != null,
            "Call PictureBoxImageHolder.RecreateImage before other method call!");
    }

    public Result<None> UpdateUi()
    {
        Refresh();
        Application.DoEvents();
        return Result.Ok();
    }

    public Result<None> RecreateImage(ImageSettings imageSettings)
    {
        Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        return Result.Ok();
    }

    public Result<None> SaveImage(string fileName)
    {
        return ValidateImageCreation()
            .Then(x => x.Save(fileName));
    }
}
