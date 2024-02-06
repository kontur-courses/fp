using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud.ColorGenerators;
using TagsCloud.ConsoleCommands;
using TagsCloud.Entities;


namespace TagsCloud.TagsCloudPainters;

public class SimplePainter : IPainter
{
    private readonly IColorGenerator _colorGenerator;
    private readonly string _filename;
    private readonly Size _imageSize;
    private readonly string _color;

    public SimplePainter(IColorGenerator colorGenerator, Options options)
    {
        this._colorGenerator = colorGenerator;
        this._filename = options.OutputFile;
        this._imageSize = options.ImageSize;
        this._color = options.Background;
    }

    public Result<None> DrawCloud(Cloud cloud)
    {
        var tags = cloud.Tags;
        var cloudSize = cloud.CloudSize;
        if (!Color.FromName(_color).IsKnownColor)
        {
            return Result.Fail<None>($"There is no color with this name: {_color}");
        }
        var backgroundColor = Color.FromName(_color);

        if (!cloud.Tags.Any())
            return Result.Fail<None>("Painter can't draw empty tag collection");

        if (!_imageSize.IsEmpty && (_imageSize.Height < cloudSize.Height || _imageSize.Width < cloudSize.Width))
            return Result.Fail<None>("Tag cloud cannot fit given size");
        cloudSize = _imageSize.IsEmpty ? cloudSize : _imageSize;
        
        using var bitmap = new Bitmap(cloudSize.Width, cloudSize.Height);
        using var g = Graphics.FromImage(bitmap);
        g.Clear(backgroundColor);
        foreach (var tag in tags)
        {
            var color = _colorGenerator.GetTagColor(tag).GetValueOrThrow();
            var brush = new SolidBrush(color);
            g.DrawString(tag.Content, tag.Font, brush,
                GetTagPositionOnImage(tag.TagRectangle.Location, _imageSize));
        }

        SaveImageToFile(bitmap, _filename);

        return Result.Ok();
    }

    private Point GetTagPositionOnImage(Point position, Size size)
    {
        var x = position.X + size.Width / 2;
        var y = position.Y + size.Height / 2;

        return new Point(x, y);
    }

    private void SaveImageToFile(Bitmap bitmap, string filename)
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        Console.WriteLine(projectDirectory);
        bitmap.Save(filename, ImageFormat.Png);
    }
}