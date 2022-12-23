using System.Drawing;

namespace TagCloud.App.CloudCreatorDriver.RectanglesLayouters.SpiralCloudLayouters;

public class SpiralCloudLayouterSettings : ICloudLayouterSettings
{
    public Point Center { get; set; }
    public double SpiralStep { get; }
    public double RotationStep { get; }
        
    public SpiralCloudLayouterSettings(Point center, double spiralStep, double rotationStep)
    {
        Center = center;
        SpiralStep = spiralStep;
        RotationStep = rotationStep;
    }
}