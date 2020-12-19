namespace TagsCloud.Spirals
{
    public class SpiralSettings
    {
        private double spiralParameter = 0.005;
        private const double MinSpiralParameter = 0.001;
        private const double MaxSpiralParameter = 5;

        public double SpiralParameter
        {
            get => spiralParameter;
            set
            {
                if (value > MaxSpiralParameter)
                    spiralParameter = MaxSpiralParameter;
                else if (value < MinSpiralParameter)
                    spiralParameter = MinSpiralParameter;
                else spiralParameter = value;
            }
        }
    }
}