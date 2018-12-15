using System.Collections.Generic;
using System.Linq;
using NHunspell;
using TagCloud.ExceptionHandler;

namespace TagCloud.Words
{
    public class BasicFormConverter : IWordProcessor
    {
        private readonly NHunspellSettings settings;
        private readonly IExceptionHandler exceptionHandler;

        public BasicFormConverter(NHunspellSettings settings, IExceptionHandler exceptionHandler)
        {
            this.settings = settings;
            this.exceptionHandler = exceptionHandler;
        }
        
        public IEnumerable<string> Preprocess(IEnumerable<string> words)
        {
            var convertedWords = Result.Of(()=> new Hunspell(settings.AffFile, settings.DictFile))
                .Then(hs => (from word in words
                    select hs.Stem(word).Any()
                        ? hs.Stem(word).First()
                        : word).ToList())
                .RefineError("Failed, trying to init NHunspell")
                .OnFail(exceptionHandler.HandleException);
            return convertedWords.Value;
        }
    }
}