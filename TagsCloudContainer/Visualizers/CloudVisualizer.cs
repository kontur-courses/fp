using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Visualizers;

public class CloudVisualizer : ICloudVisualizer
{
    private readonly IImageSettings settings;

    public CloudVisualizer(IImageSettings settings)
    {
        this.settings = settings;
    }

    public Result<Image> DrawImage(ITagCloud cloud)
    {
        var result = InitializeImage()
            .Then(DrawBackground);
        foreach (var tag in cloud.Tags)
        {
            result = result.Then(image => DrawTag(image, tag));
        }

        return result;
    }

    private Result<Image> InitializeImage()
    {
        return new Image<Rgba64>(settings.ImageSize.Width, settings.ImageSize.Height);
    }

    private Result<Image> DrawTag(Image image, Tag tag)
    {
        var location = new PointF(tag.Rectangle.Location.X, tag.Rectangle.Location.Y);
        if (settings?.TextOptions?.Font is null)
        {
            return Result.Fail<Image>("Не найден шрифт для отрисовки.");
        }

        try
        {
            image.Mutate(ctx =>
            {
                ctx.DrawText(tag.Word, settings.TextOptions.Font, settings.PrimaryColor, location);
            });
        }
        catch (Exception e)
        {
            return Result.Fail<Image>($"Произошла ошибка при отрисовке тега: {e}");
        }

        return image;
    }

    private Result<Image> DrawBackground(Image image)
    {
        try
        {
            image.Mutate(ctx => { ctx.Fill(settings.BackgroundColor); });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла ошибка при отрисовке: {e}");
        }

        return image;
    }
}