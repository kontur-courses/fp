using System.Drawing;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.ImageSettings;

namespace TagsCloudVisualization.Drawer;

public class ImageDrawer : IDrawer
{
    private readonly IImageSettingsProvider settingsProvider;
    private readonly AbstractImageSaver imageSaver;

    public ImageDrawer(AbstractImageSaver imageSaver, IImageSettingsProvider settingsProvider)
    {
        this.settingsProvider = settingsProvider;
        this.imageSaver = imageSaver;
    }

    public Result<None> Draw(IReadOnlyCollection<IDrawImage> drawImages, string filePath)
    {
        return settingsProvider.GetSettings().Then(settings =>
        {
            using var image = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);
            using var graphics = Graphics.FromImage(image);
            graphics.Clear(settings.BackgroundColor);
            return Draw(drawImages, filePath, settings, graphics, image);
        });
    }

    private Result<None> Draw(IReadOnlyCollection<IDrawImage> drawImages, string filePath, ImageSettings.ImageSettings settings, Graphics graphics,
        Bitmap image)
    {
        var bounds = new Rectangle(Point.Empty, settings.ImageSize);
        
        var shifted = drawImages.Select(tag => tag.Offset(settings.ImageSize / 2)).ToList();

        return shifted.AsResult()
            .Validate(tags => Validate(tags, bounds), "Rectangles don't fit")
            .Then(() => Draw(shifted, graphics))
            .Then(() => imageSaver.Save(filePath, image))
            .RefineError("Failed to draw image");
    }

    private Result<None> Draw(IReadOnlyCollection<IDrawImage> drawImages, Graphics graphics)
    {
        return Result.Of(() =>
        {
            foreach (var drawable in drawImages)
            {
                drawable.Draw(graphics);
            }
        });
    }

    private bool Validate(IReadOnlyCollection<IDrawImage> tagImages, Rectangle bounds)
    {
        var result = true;
        foreach (var tag in tagImages)
        {
            if (!bounds.Contains(tag.Bounds))
                result = false;
        }

        return result;
    }
}