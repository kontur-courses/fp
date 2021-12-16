namespace App.Infrastructure
{
    public interface ICloudGenerator
    {
        Result<CloudVisualization> GenerateCloud();
    }
}