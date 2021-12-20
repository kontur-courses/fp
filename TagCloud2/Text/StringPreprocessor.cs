﻿using ResultOf;

namespace TagCloud2
{
    public class StringPreprocessor : IStringPreprocessor
    {
        private readonly ISillyWordsFilter sillyRemover;
        private readonly ISillyWordSelector sillySelector;
        public Result<string> PreprocessString(string input)
        {
            return sillyRemover.FilterSillyWords(input, sillySelector);
        }

        public StringPreprocessor(ISillyWordsFilter remover, ISillyWordSelector selector)
        {
            sillySelector = selector;
            sillyRemover = remover;
        }
    }
}
