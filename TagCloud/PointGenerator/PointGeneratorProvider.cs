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
        return registeredGenerators.ContainsKey(generatorName)
            ? Result.Ok(registeredGenerators[generatorName])
            : Result.Fail<IPointGenerator>($"{generatorName} layouter is not supported");
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