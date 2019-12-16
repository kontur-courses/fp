using System;

namespace TextConfiguration.WordFilters
{
    public class EmptyWordFilter : IWordFilter
    {
        public Result<bool> ShouldExclude(string word)
        {
            return String.IsNullOrWhiteSpace(word);
        }
    }
}
