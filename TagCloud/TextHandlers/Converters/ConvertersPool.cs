using System;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud.TextHandlers.Converters
{
    public class ConvertersPool : IConvertersPool
    {
        private readonly List<Func<string, Result<string>>> converters;

        public ConvertersPool(IConverter[] converters)
        {
            this.converters = new List<Func<string, Result<string>>>();
            foreach (var converter in converters)
            {
                this.converters.Add(converter.Convert);
            }
        }

        public ConvertersPool Using(Func<string, Result<string>> converter)
        {
            converters.Add(converter);
            return this;
        }

        public Result<IEnumerable<string>> Convert(IEnumerable<string> words)
        {
            return words
                .AsResult()
                .Then(w => w.Select(Convert))
                .Then(results => results.Select(w => w.Value));
        }

        public Result<string> Convert(string word)
        {
            var converted = word.AsResult();
            foreach (var converter in converters)
            {
                converted.Then(converter);
            }

            return converted;
        }
    }
}