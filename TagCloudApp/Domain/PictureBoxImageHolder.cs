using System.Drawing.Imaging;
using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudApp.Domain;

public class PictureBoxImageHolder : PictureBox, IImageHolder
{
    private readonly IImageSaverProvider _imageSaverProvider;
    private readonly IImageSettingsProvider _imageSettingsProvider;
    private readonly IErrorHandler _errorHandler;

    public PictureBoxImageHolder(
        IImageSaverProvider imageSaverProvider,
        IImageSettingsProvider imageSettingsProvider,
        IErrorHandler errorHandler
    )
    {
        _imageSaverProvider = imageSaverProvider;
        _imageSettingsProvider = imageSettingsProvider;
        _errorHandler = errorHandler;
    }

    public Size GetImageSize()
    {
        FailIfNotInitialized();
        return Image.Size;
    }

    public Graphics StartDrawing()
    {
        FailIfNotInitialized();
        RecreateImage();
        return Graphics.FromImage(Image);
    }

    private void FailIfNotInitialized()
    {
        if (Image == null)
            throw new InvalidOperationException("Call PictureBoxImageHolder.RecreateImage before other method call!");
    }

    public void UpdateUi()
    {
        Refresh();
        Application.DoEvents();
    }

    public void RecreateImage()
    {
        var imageSettings = _imageSettingsProvider.GetImageSettings();
        Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
    }

    public void SaveImage()
    {
        FailIfNotInitialized();
        _imageSaverProvider.GetSaver()
            .Then(saver => saver.SaveImage(Image))
            .OnFail(_errorHandler.HandleError);
    }
}