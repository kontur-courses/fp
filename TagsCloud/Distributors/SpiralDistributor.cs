using System.Drawing;
using TagsCloud.Options;


namespace TagsCloud.Distributors;

public class SpiralDistributor : IDistributor
{
    public double Angle = 0;
    public double Radius = 0;
    public double AngleStep = 0.1;
    public double RadiusStep = 0.1;
    public Point Center { get; private set; }

    public SpiralDistributor(LayouterOptions options)
    {
        this.Center = options.Center;
        /*
         * Radius = 0;
         * Angle = 0;
         * this.AngleStep = 0.1;
         * this.RadiusStep = 0.1;
         */
       
    }


    public Result<Point> GetNextPosition()
    {
        var x = Radius * Math.Cos(Angle) + Center.X;
        var y = Radius * Math.Sin(Angle) + Center.Y;

        Angle += AngleStep;

        if (Angle >= Math.PI * 2)
        {
            Angle -= 2 * Math.PI;
            Radius += RadiusStep;
        }

        var point = new Point((int)x, (int)y);
        return point;
    }
}