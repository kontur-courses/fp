using System;
using System.Collections.Generic;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.WordFilter
{
    public class Filters
    {
        private readonly Dictionary<string, Func<FilterSettings, IFilter>> filtersDictionary;
        private readonly FilterSettings filterSettings;

        public Filters(FilterSettings filterSettings)
        {
            this.filterSettings = filterSettings;

            filtersDictionary = new Dictionary<string, Func<FilterSettings, IFilter>>
            {
                {"length", filterSett => new LengthWordFilter(filterSett)},
                {"boring", filterSett => new BoringWordFilter(filterSett)}
            };
        }

        public IFilter[] GetFiltersByName(IEnumerable<string> filters)
        {
            var resultFilters = new List<IFilter>();
            foreach (var filter in filters) resultFilters.Add(filtersDictionary[filter](filterSettings));
            return resultFilters.ToArray();
        }
    }
}