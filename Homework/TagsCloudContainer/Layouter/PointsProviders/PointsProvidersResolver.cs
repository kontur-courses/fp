using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.Layouter.PointsProviders
{
    public class PointsProvidersResolver : IResolver<LayoutAlrogorithm, IPointsProvider>
    {
        private readonly Dictionary<LayoutAlrogorithm, IPointsProvider> resolver;


        public PointsProvidersResolver(params IPointsProvider[] pointsProvider)
        {
            resolver = pointsProvider.ToDictionary(x => x.AlghorithmName);
        }

        public Result<IPointsProvider> Get(LayoutAlrogorithm algorithm)
        {
            if (resolver.ContainsKey(algorithm))
                return Result.Ok(resolver[algorithm]);
            return Result.Fail<IPointsProvider>($"{algorithm} does not exist");
        }
    }
}