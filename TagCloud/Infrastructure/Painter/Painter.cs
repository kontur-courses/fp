﻿using System.Drawing;
using TagCloud.App.UI.Console.Common;
using TagCloud.Infrastructure.Layouter;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.Painter;

public class Painter : IPainter
{
    private readonly ICloudLayouterFactory layouterFactory;
    private readonly IPalette palette;
    private readonly IAppSettings settings;

    public Painter(IPalette palette, ICloudLayouterFactory layouterFactory, IAppSettings settings)
    {
        ValidateSettings(settings);
        this.palette = palette;
        this.layouterFactory = layouterFactory;
        this.settings = settings;
    }

    public Result<Bitmap> CreateImage(Dictionary<string, int> weightedWords)
    {
        if (weightedWords == null || !weightedWords.Any())
            return Result.Fail<Bitmap>("Impossible to save an empty tag cloud");

        var bitmap = new Bitmap(settings.ImageWidth, settings.ImageHeight);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(palette.BackgroundColor);

        var tags = GetTags(weightedWords, graphics);

        if (!tags.IsSuccess)
            return Result.Fail<Bitmap>(tags.Error);

        var scale = CalculateScale(settings.ImageWidth, settings.ImageHeight, tags.Value);

        foreach (var tag in tags.Value)
        {
            using var brush = new SolidBrush(palette.MainColor);
            using var font = new Font(settings.FontName, tag.FontSize * scale);
            graphics.DrawString(tag.Word, font, brush, RescaleRectangle(tag.Position, scale));
        }

        return Result.Ok(bitmap);
    }

    private Result<List<Tag>> GetTags(Dictionary<string, int> weightedWords, Graphics graphics)
    {
        var positionedWords = new List<Tag>();
        var layouter = layouterFactory.Create(settings.LayouterName);

        foreach (var (word, weight) in weightedWords)
        {
            var fontSize = 10 + weight / 2;
            using var font = new Font(settings.FontName, fontSize);
            var size = graphics.MeasureString(word, font);
            var rectangle = layouter.PutNextRectangle(Size.Ceiling(size));

            if (!rectangle.IsSuccess)
                return Result.Fail<List<Tag>>(rectangle.Error);

            positionedWords.Add(new Tag(word, rectangle.Value, fontSize));
        }

        return Result.Ok(positionedWords);
    }

    private static void ValidateSettings(IImageSettingsProvider settings)
    {
        if (settings.ImageHeight <= 0 || settings.ImageWidth <= 0)
            throw new ArgumentException($"Image sizes must be great than zero, but was {settings.ImageWidth}x{settings.ImageHeight}");
    }

    private static float CalculateScale(int imageWidth, int imageHeight, IReadOnlyCollection<Tag> tags)
    {
        var layoutSize = CalculateLayoutSize(tags);
        var scale = Math.Min((float)imageHeight / layoutSize.Height, (float)imageWidth / layoutSize.Width);

        return scale < 1 ? scale : 1;
    }

    private static RectangleF RescaleRectangle(RectangleF rectangle, float scale)
    {
        var size = new SizeF(rectangle.Size.Width * scale, rectangle.Size.Height * scale);
        var location = new PointF(rectangle.X * scale, rectangle.Y * scale);

        return new RectangleF(location, size);
    }

    private static Size CalculateLayoutSize(IReadOnlyCollection<Tag> positionedWords)
    {
        var maxX = positionedWords.Max(x => x.Position.X + x.Position.Size.Width);
        var maxY = positionedWords.Max(x => x.Position.Y);
        var minX = positionedWords.Min(x => x.Position.X);
        var minY = positionedWords.Min(x => x.Position.Y - x.Position.Size.Height);
        var width = maxX - minX;
        var height = maxY - minY;

        return new Size(width, height);
    }
}