using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TagCloud.selectors
{
    public class WordConverter : IConverter<IEnumerable<string>>
    {
        private readonly List<IConverter<string>> singleConverters;

        public WordConverter([NotNull]List<IConverter<string>> singleConverters)
        {
            this.singleConverters = singleConverters;
        }

        public Result<IEnumerable<string>> Convert(IEnumerable<string> source) =>
            Result.Of(() => source.Select(word =>
                    singleConverters.Aggregate(word,
                        (current, converter) => converter.Convert(current).GetValueOrThrow())),
                ResultErrorType.ConverterError
            );
    }
}