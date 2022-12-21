using System.Collections.Generic;
using System.Linq;
using TagCloud.ResultMonade;

namespace TagCloud.WordConverter
{
    public class ConvertersExecutor : IConvertersExecutor
    {
        private readonly IEnumerable<IWordConverter> Converters;

        public ConvertersExecutor(IEnumerable<IWordConverter> converters)
        {
            Converters = converters;
        }

        public Result<List<string>> Convert(IEnumerable<string> words)
        {
            return words.Select(word => Converters.Aggregate(word, (current, converter) => converter.Convert(current).Value))
                        .ToList();
        }
    }
}
