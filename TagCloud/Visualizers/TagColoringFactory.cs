using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.ResultMonad;

namespace TagCloud.Visualizers
{
    public class TagColoringFactory : ITagColoringFactory
    {
        private static readonly Dictionary<string, Func<IEnumerable<Color>, ITagColoring>> factory = new()
        {
            {"alt", colors => new AlternatingTagColoring(colors)},
            {"random", colors => new RandomTagColoring(colors)}
        };

        public Result<ITagColoring> Create(string algorithmName, IEnumerable<Color> colors)
        {
            if (factory.TryGetValue(algorithmName, out var algorithm))
                return Result.Ok(algorithm(colors));
            return Result.Fail<ITagColoring>("Wrong coloring algorithm name." +
                                             " Please use one of this names: "+
                                             string.Join(" ", factory.Keys));
        }
    }
}
