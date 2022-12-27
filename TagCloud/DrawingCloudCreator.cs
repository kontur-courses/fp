﻿using System.Drawing;
using FluentResults;
using TagCloud.Abstractions;

namespace TagCloud;

public class DrawingCloudCreator : ICloudCreator
{
    private readonly ICloudDrawer drawer;
    private readonly ICloudLayouter layouter;

    public DrawingCloudCreator(ICloudLayouter layouter, ICloudDrawer drawer)
    {
        this.layouter = layouter;
        this.drawer = drawer;
    }

    public Result<IEnumerable<IDrawableTag>> CreateTagCloud(IEnumerable<ITag> tags)
    {
        var tagsArray = tags.ToArray();

        if (!tagsArray.Any()) return Array.Empty<IDrawableTag>();

        if (tagsArray.Length == 1)
        {
            var tag = tagsArray[0];
            var fontsize = drawer.Settings.MaxFontSize;
            return new[] { new DrawableTag(tag, fontsize, GetPoint(tag.Text, fontsize)) };
        }

        var maxCount = tagsArray.Max(t => t.Weight);
        var minCount = tagsArray.Min(t => t.Weight);

        if (minCount <= 0)
            return Result.Fail($"Weight of Tag should be greater than 0, but {minCount}");

        var drawableTags = tagsArray.Select(t =>
        {
            var fontsize = GetFontSize(t.Weight, minCount, maxCount);
            return new DrawableTag(t, fontsize, GetPoint(t.Text, fontsize));
        });

        return drawableTags.ToList();
    }

    private Point GetPoint(string word, int fontsize)
    {
        using var font = new Font(drawer.Settings.FontFamily, fontsize);
        return layouter.PutNextRectangle(drawer.Graphics.MeasureString(word, font).ToSize()).Value.Location;
    }

    private int GetFontSize(int count, int minCount, int maxCount)
    {
        var maxSize = drawer.Settings.MaxFontSize;
        var minSize = drawer.Settings.MinFontSize;
        return -(int)((double)(minSize * maxCount - minSize * count + maxSize * count - maxSize * minCount) /
                      (minCount - maxCount));
    }
}