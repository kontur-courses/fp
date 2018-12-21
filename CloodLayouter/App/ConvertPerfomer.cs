using System.Collections.Generic;
using CloodLayouter.Infrastructer;

namespace CloodLayouter.App
{
    public class ConvertPerfomer : IProvider<IEnumerable<Tag>>
    {
        private readonly IProvider<IEnumerable<string>> wordProvider;
        private readonly IConverter<IEnumerable<string>, IEnumerable<string>> wordSelector;
        private readonly IConverter<IEnumerable<string>, IEnumerable<Tag>> wordToTagConverter;

        public ConvertPerfomer(IProvider<IEnumerable<string>> wordProvider,
            IConverter<IEnumerable<string>, IEnumerable<string>> wordSelector,
            IConverter<IEnumerable<string>, IEnumerable<Tag>> wordToTagConverter)
        {
            this.wordProvider = wordProvider;
            this.wordSelector = wordSelector;
            this.wordToTagConverter = wordToTagConverter;
        }

        public IEnumerable<Tag> Get()
        {
            return wordToTagConverter.Convert(wordSelector.Convert(wordProvider.Get()));
        }
    }
}