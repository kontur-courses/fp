using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization.Interfaces
{
    public interface IPointGeneratorDetector
    {
        IPointGenerator GetPointGenerator(string name);
        Result<string> ValidatePointGeneratorIsSupported(string pointGenerator);
    }
}