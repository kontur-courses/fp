using System.Drawing;
using System.Drawing.Text;

public class TagCloudRenderer : ITagCloudRenderer, IDisposable
{
    private readonly RenderOptions options;
    private readonly IColorProvider colorProvider;

    private readonly Graphics graphics;
    private readonly Bitmap bitmap;


    public TagCloudRenderer(RenderOptions options, IColorProvider colorProvider)
    {
        bitmap = new Bitmap(options.ImageSize.Width, options.ImageSize.Height);
        graphics = Graphics.FromImage(bitmap);

        this.colorProvider = colorProvider;
        this.options = options;
    }

    public Size GetStringSize(string str, int fontSize)
    {
        var newFont = GetFontWithAdjastentSize(fontSize);
        return Size.Truncate(graphics.MeasureString(str, newFont));
    }
    private Font GetFontWithAdjastentSize(int fontSize) => new Font(options.FontBase.Name, fontSize, options.FontBase.Style);

    public Result<Bitmap> Render(WordLayout[] wordLayouts)
    {
        graphics.Clear(options.ColorScheme.BackgroundColor);

        foreach (var layout in wordLayouts) 
        {
            var color = colorProvider.GetColor(layout);

            if (!color.IsSuccess) 
                return Result.Fail<Bitmap>(color.Error);

            var brush = new SolidBrush(color.Value);
            var adjFont = GetFontWithAdjastentSize(layout.FontSize);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.DrawString(layout.Content, adjFont, brush, layout.Box.Location);
        }

        return Result.Ok(bitmap);
    }

    public void Dispose()
    {
        graphics.Dispose();
        bitmap.Dispose();
    }
}