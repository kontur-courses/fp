using Aspose.Drawing;
using Aspose.Drawing.Imaging;
using TagCloud.Utils.Files.Interfaces;
using TagCloud.Utils.Images.Interfaces;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Images;

public class ImageWorker : IImageWorker
{
    private readonly IFileService _fileService;
    
    public ImageWorker(IFileService fileService)
    {
        _fileService = fileService;
    }
    
    public Result<None> SaveImage(Image image, string path, ImageFormat imageFormat, string filename)
    {
        return _fileService.CreateDirectory(path)
            .Then(() => Path.Combine(path, filename + "." + imageFormat.ToString().ToLower()))
            .Then(combined => image.Save(combined, imageFormat));
    }
}