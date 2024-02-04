using Results;
using System.Drawing;
using TagsCloudVisualization.ColorGenerators;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.Visualizers;

public class Visualizer : IVisualizer
{
    private readonly ImageSettings imageSettings;
    private readonly BackgroundSettings backgroundSettings;
    private readonly IColorGenerator[] colorGenerators;

    public Visualizer(ImageSettings imageSettings, BackgroundSettings backgroundSettings, IColorGenerator[] generators)
    {
        this.imageSettings = imageSettings;
        colorGenerators = generators;
        this.backgroundSettings = backgroundSettings;
    }

    public Result<Bitmap> Vizualize(IEnumerable<Result<Tag>> tags)
    {
        if (!imageSettings.Width.IsSuccess)
            return Result.Fail<Bitmap>(imageSettings.Width.Error);
        if (!imageSettings.Height.IsSuccess)
            return Result.Fail<Bitmap>(imageSettings.Height.Error);
        if (!backgroundSettings.BackgroundColor.IsSuccess)
            return Result.Fail<Bitmap>(backgroundSettings.BackgroundColor.Error);
        var generator = GetColorGenerator();
        if (!generator.IsSuccess)
            return Result.Fail<Bitmap>(generator.Error);

        var bitmap = new Bitmap(imageSettings.Width.Value, imageSettings.Height.Value);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(backgroundSettings.BackgroundColor.Value);

        foreach (var tag in tags)
        {
            if (!tag.IsSuccess)
                return Result.Fail<Bitmap>(tag.Error);
            if (!IsRectangleInImage(tag.Value.Rectangle))
                return Result.Fail<Bitmap>("Tag cloud doesn't fit in image size");
            graphics.DrawString(tag.Value.Content, 
                new Font(tag.Value.Font, tag.Value.Size), 
                new SolidBrush(generator.Value.GetColor()),
                tag.Value.Rectangle);
        }
        return bitmap;
    }

    public Result<IColorGenerator> GetColorGenerator()
    {
        var generator = colorGenerators.Where(g => g.Match()).FirstOrDefault();
        return generator is null
            ? Result.Fail<IColorGenerator>("Can't find color") 
            : Result.Ok(generator);
    }

    private bool IsRectangleInImage(Rectangle rectangle)
    {
        return rectangle.Left >= 0 && rectangle.Right <= imageSettings.Width.Value 
            && rectangle.Top >= 0 && rectangle.Bottom <= imageSettings.Height.Value;
    }
}