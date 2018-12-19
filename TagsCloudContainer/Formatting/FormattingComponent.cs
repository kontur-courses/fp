using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TagsCloudContainer.Formatting
{
    public class FormattingComponent
    {
        private readonly List<IWordsFormatter> formatters;

        public FormattingComponent(IWordsFormatter[] formatters)
        {
            this.formatters = formatters.ToList();
        }


        public ReadOnlyCollection<string> FormatWords(ReadOnlyCollection<string> words)
        {
            foreach (var formatter in formatters)
            {
                words = formatter.Format(words);
            }

            return words;
        }

        public void AddFormatter(IWordsFormatter formatter)
        {
            formatters.Add(formatter);
        }
    }
}