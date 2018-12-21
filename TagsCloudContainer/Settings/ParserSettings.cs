using System.Collections.Generic;
using TagsCloudContainer.Processing.Converting;
using TagsCloudContainer.Processing.Filtering;

namespace TagsCloudContainer.Settings
{
    public class ParserSettings
    {
        public readonly IReadOnlyCollection<IWordFilter> Filters;
        public readonly IReadOnlyCollection<IWordConverter> Converters;

        public ParserSettings(IWordFilter[] filters, IWordConverter[] converters)
        {
            Filters = filters;
            Converters = converters;
        }
    }
}