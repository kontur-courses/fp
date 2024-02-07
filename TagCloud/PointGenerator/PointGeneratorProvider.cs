using ResultOf;

namespace TagCloud.PointGenerator;

public class PointGeneratorProvider : IPointGeneratorProvider
{
    private Dictionary<string, IPointGenerator> registeredGenerators;

    public PointGeneratorProvider(IEnumerable<IPointGenerator> generators)
    {
        registeredGenerators = ArrangeLayouters(generators);
    }

    public Result<IPointGenerator> CreateGenerator(string generatorName)
    {
        var availableLayouters = string.Join("", registeredGenerators.Select(pair => pair.Key));

        return registeredGenerators.ContainsKey(generatorName)
            ? Result.Ok(registeredGenerators[generatorName])
            : Result.Fail<IPointGenerator>(
                $"{generatorName} layouter is not supported. Available layouters are: {availableLayouters}");
    }

    private Dictionary<string, IPointGenerator> ArrangeLayouters(IEnumerable<IPointGenerator> generators)
    {
        var generatorsDictionary = new Dictionary<string, IPointGenerator>();
        foreach (var generator in generators)
        {
            generatorsDictionary[generator.GeneratorName] = generator;
        }

        return generatorsDictionary;
    }
}