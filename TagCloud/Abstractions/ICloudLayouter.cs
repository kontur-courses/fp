using System.Collections.Immutable;
using System.Drawing;
using FluentResults;

namespace TagCloud.Abstractions;

public interface ICloudLayouter
{
    public Point Center { get; }
    public ImmutableArray<Rectangle> Rectangles { get; }
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}