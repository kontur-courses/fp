namespace TagsCloudResult.CircularCloudLayouter
{
    public class Direction : IDirection<double>
    {
        private readonly double angleShift;
        private double currentAlpha;

        public Direction(double angleShift = 1)
        {
            this.angleShift = angleShift;
        }

        public double GetNextDirection()
        {
            var oldAlpha = currentAlpha;
            currentAlpha = (currentAlpha + angleShift).AngleToStandardValue();

            return oldAlpha;
        }
    }
}