namespace TagsCloudVisualization.Configurations
{
    public class DistributionConfiguration
    {
        public static DistributionConfiguration Default => 
            new (0.1f, 5.0f, 2.5f);
        
        public float ShiftAngle { get; }
        public float ShiftX { get; }
        public float ShiftY { get; }

        private DistributionConfiguration(float shiftAngle, float shiftX, float shiftY)
        {
            ShiftAngle = shiftAngle;
            ShiftX = shiftX;
            ShiftY = shiftY;
        }
        
        public static DistributionConfiguration Create(float shiftAngle, float shiftX, float shiftY)
        {
            var distributionConfiguration = new DistributionConfiguration(shiftAngle, shiftX, shiftY);

            return distributionConfiguration.Validate().GetValueOrThrow();
        }

        private Result<DistributionConfiguration> Validate()
        {
            if (ShiftAngle <= 0 || ShiftX <= 0 || ShiftY <= 0)
                return Result.Fail<DistributionConfiguration>("The arguments are incorrect");
            
            return Result.Ok(this);
        }
    }
}