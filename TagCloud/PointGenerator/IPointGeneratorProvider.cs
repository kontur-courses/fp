using ResultOf;

namespace TagCloud.PointGenerator;

public interface IPointGeneratorProvider
{
    Result<IPointGenerator> CreateGenerator(string generatorName);
}