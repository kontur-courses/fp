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

    public Result<Bitmap> Vizualize(Result<IList<Tag>> tags)
    {
        var checkImageSettings = imageSettings.Check();
        if (!checkImageSettings.IsSuccess)
            return Result.Fail<Bitmap>(checkImageSettings.Error);
        var checkBackgroundSettings = backgroundSettings.Check();
        if (!checkBackgroundSettings.IsSuccess)
            return Result.Fail<Bitmap>(checkBackgroundSettings.Error);
        var generator = GetColorGenerator();
        if (!generator.IsSuccess)
            return Result.Fail<Bitmap>(generator.Error);
        if (!tags.IsSuccess)
            return Result.Fail<Bitmap>(tags.Error);

        var bitmap = new Bitmap(imageSettings.Width, imageSettings.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.FromName(backgroundSettings.BackgroundColor));

        foreach (var tag in tags.Value)
        {
            if (!IsRectangleInImage(tag.Rectangle))
                return Result.Fail<Bitmap>("Tag cloud doesn't fit in image size");
            graphics.DrawString(tag.Content, 
                new Font(tag.Font, tag.Size), 
                new SolidBrush(generator.Value.GetColor()),
                tag.Rectangle);
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
        return rectangle.Left >= 0 && rectangle.Right <= imageSettings.Width 
            && rectangle.Top >= 0 && rectangle.Bottom <= imageSettings.Height;
    }
}