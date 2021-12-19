using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.Visualizer.ColorGenerators
{
    public class ColorGeneratorsResolver : IResolver<PalleteType, IColorGenerator>
    {
        private readonly Dictionary<PalleteType, IColorGenerator> resolver;

        public ColorGeneratorsResolver(IColorGenerator[] colorGenerators)
        {
            resolver = colorGenerators.ToDictionary(x => x.PalleteType);
        }

        public Result<IColorGenerator> Get(PalleteType palleteType)
        {
            if (resolver.ContainsKey(palleteType))
                return Result.Ok(resolver[palleteType]);
            return Result.Fail<IColorGenerator>($"{palleteType} not found");
        }
    }
}