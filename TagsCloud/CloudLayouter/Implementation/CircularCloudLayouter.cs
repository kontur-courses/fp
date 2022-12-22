using System.Drawing;
using TagCloud.Creators;
using TagCloud.Creators.Implementation;
using TagCloud.FigurePatterns;
using TagCloud.FigurePatterns.Implementation;
using TagCloud.ResultImplementation;

namespace TagCloud.CloudLayouter.Implementation;

public sealed class CircularCloudLayouter : CloudLayouter<Rectangle>
{
    private readonly ICreator<Rectangle> creator;
    private readonly IFigurePatternPointProvider figurePatternPointProvider;

    public CircularCloudLayouter(IFigurePatternPointProvider provider, ICreator<Rectangle> creator)
    {
        figurePatternPointProvider = provider;
        this.creator = creator;
    }

    public CircularCloudLayouter(Point center, double figureStep = 1)
    {
        figurePatternPointProvider = new SpiralPatterPointProvider(center, figureStep);
        creator = new RectangleCreator();
    }

    public override Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            return Result.Fail<Rectangle>($"Incorrect width or height: {nameof(rectangleSize)}");

        var figure = GetNextFigure(rectangleSize);
        Figures.Add(figure);
        return Result.Ok(figure);
    }

    private Rectangle GetNextFigure(Size size)
    {
        while (true)
        {
            var point = figurePatternPointProvider.GetNextPoint();
            var figure = creator.Place(point, size);
            if (Figures.Any(fig => fig.IntersectsWith(figure)))
                continue;

            figurePatternPointProvider.Restart();
            return figure;
        }
    }
}
