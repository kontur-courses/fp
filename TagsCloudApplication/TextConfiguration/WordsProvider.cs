using System.Collections.Generic;
using TextConfiguration.TextReaders;

namespace TextConfiguration
{
    public class WordsProvider : IWordsProvider
    {
        private readonly ITextReader reader;
        private readonly ITextPreprocessor preprocessor;

        public WordsProvider(ITextReader reader, ITextPreprocessor preprocessor)
        {
            this.reader = reader;
            this.preprocessor = preprocessor;
        }

        public Result<List<string>> ReadWordsFromFile(string filePath)
        {
            return reader.ReadText(filePath)
                .Then(text => preprocessor.PreprocessText(text));
        }
    }
}
