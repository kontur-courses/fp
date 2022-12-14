using System.Drawing;
using System.Runtime.InteropServices;
using TagCloud.ColoringAlgorithm;
using TagCloud.Infrastructure;
using TagCloud.LayoutAlgorithm;

namespace TagCloud.ImageGenerator;

public class BitmapImageGenerator : IImageGenerator
{
    private readonly Size size;
    private readonly IColoringAlgorithm coloringAlgorithm;
    private readonly FontProvider fontProvider;
    private readonly ILayoutAlgorithm layoutAlgorithm;

    public BitmapImageGenerator(Size size, IColoringAlgorithm coloringAlgorithm, FontProvider fontProvider,
        ILayoutAlgorithm layoutAlgorithm)
    {
        this.size = size;
        this.coloringAlgorithm = coloringAlgorithm;
        this.fontProvider = fontProvider;
        this.layoutAlgorithm = layoutAlgorithm;
    }
    
    public Result<Image> GenerateImage(Tag[] tags)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new Result<Image>("OS not supported");

        var font = fontProvider.GetFont();
        if (!font.IsSuccess)
            return new Result<Image>(font.Error);
        
        var image = new Bitmap(size.Width, size.Height);

        using var graphics = Graphics.FromImage(image);
        using var backgroundBrush = new SolidBrush(Color.White);
        
        graphics.FillRectangle(backgroundBrush,0, 0, image.Width, image.Height);
        var colors = coloringAlgorithm.GetColors(tags.Length);
        var i = 0;
        foreach (var tag in tags)
        {
            var tagFont = new Font(font.Value!.FontFamily, tag.Size);
            var measuredTag = graphics.MeasureString(tag.Word, tagFont);
            var tagSize = new Size((int)Math.Ceiling(measuredTag.Width), (int)Math.Ceiling(measuredTag.Height));
            var position = layoutAlgorithm.PutNextRectangle(tagSize).Location;

            if (position.X < 0 || position.X + tagSize.Width > size.Width 
                               || position.Y < 0 || position.Y + tagSize.Height > size.Height)
                return new Result<Image>($"Image size (width: {size.Width}, height: {size.Height}) is too small.");
            
            using var tagBrush = new SolidBrush(colors[i++]);
            graphics.DrawString(tag.Word, tagFont, tagBrush, position);
        }

        return image;
    }
}