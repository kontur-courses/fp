using System.Collections.Generic;
using CloodLayouter.Infrastructer;
using ResultOf;

namespace CloodLayouter.App
{
    public class ConvertPerfomer : IProvider<IEnumerable<Result<Tag>>>
    {
        private readonly IProvider<IEnumerable<Result<string>>> wordProvider;
        private readonly IConverter<IEnumerable<Result<string>>, IEnumerable<Result<string>>> wordSelector;
        private readonly IConverter<IEnumerable<Result<string>>, IEnumerable<Result<Tag>>> wordToTagConverter;

        public ConvertPerfomer(IProvider<IEnumerable<Result<string>>> wordProvider,
            IConverter<IEnumerable<Result<string>>, IEnumerable<Result<string>>> wordSelector,
            IConverter<IEnumerable<Result<string>>, IEnumerable<Result<Tag>>> wordToTagConverter)
        {
            this.wordProvider = wordProvider;
            this.wordSelector = wordSelector;
            this.wordToTagConverter = wordToTagConverter;
        }

        public IEnumerable<Result<Tag>> Get()
        {
            return wordToTagConverter.Convert(wordSelector.Convert(wordProvider.Get()));
        }
    }
}