namespace TagsCloudVisualization.Interfaces
{
    public interface ICloudParametersParser
    {
        Result<CloudParameters> Parse(Options options);
    }
}
