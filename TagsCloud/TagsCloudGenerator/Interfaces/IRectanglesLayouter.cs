namespace TagsCloudGenerator.Interfaces
{
    public interface IRectanglesLayouter : IResettable
    {
        FailuresProcessing.Result<System.Drawing.RectangleF> PutNextRectangle(System.Drawing.SizeF rectangleSize);
    }
}