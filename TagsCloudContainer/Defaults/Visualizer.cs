using ResultExtensions;
using ResultOf;
using System.Drawing;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.SettingsProviders;

namespace TagsCloudContainer.Defaults;

public class Visualizer : IVisualizer
{
    private readonly VisualizerSettings settingsProvider;
    private readonly ITagPacker tagsPacker;
    private readonly ILayouter layouter;
    private readonly IStyler styler;

    public Visualizer(VisualizerSettings settingsProvider, ITagPacker tags, ILayouter layouter, IStyler styler)
    {
        this.settingsProvider = settingsProvider;
        tagsPacker = tags;
        this.layouter = layouter;
        this.styler = styler;
    }

    public Result<Bitmap> GetBitmap()
    {
        return tagsPacker.GetTags().Then(CreateBitmap);
    }

    private Bitmap CreateBitmap(IEnumerable<ITag> tags)
    {
        var bitmap = new Bitmap(settingsProvider.Width, settingsProvider.Height);
        using var bitmapGraphics = Graphics.FromImage(bitmap);
        bitmapGraphics.SmoothingMode = settingsProvider.SmoothingMode;
        bitmapGraphics.Clear(Color.Black);
        var count = 0;
        foreach (var tag in tags.Select(styler.Style))
        {
            var size = tag.GetTrueGraphicSize(bitmapGraphics);
            var rectangle = layouter.PutNextRectangle(size);
            tag.DrawSelf(bitmapGraphics, rectangle);
            count++;
            if (settingsProvider.WordLimit != 0 && count >= settingsProvider.WordLimit)
                break;
        }

        return bitmap;
    }
}
