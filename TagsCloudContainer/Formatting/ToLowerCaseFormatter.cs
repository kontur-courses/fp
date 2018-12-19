using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TagsCloudContainer.Formatting
{
    public class ToLowerCaseFormatter : IWordsFormatter
    {
        public ReadOnlyCollection<string> Format(IEnumerable<string> words)
        {
            return new ReadOnlyCollection<string>(words.Select(x => x.ToLower()).ToList());
        }
    }
}