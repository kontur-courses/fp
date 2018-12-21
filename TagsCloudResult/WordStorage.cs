using System.Collections.Generic;
using System.Linq;

namespace TagsCloudResult
{
    class WordStorage: IWordStorage
    {
        private readonly Dictionary<string, int> wordsRegister;
        private readonly WordsCustomizer wordsCustomizer;

        public WordStorage(WordsCustomizer customizer, IEnumerable<string> wordsToHandle)
        {
            wordsRegister = new Dictionary<string, int>();
            wordsCustomizer = customizer;
            AddRange(wordsToHandle);
        }

        public Result<None> Add(string word)
        {
            var customizedWord = wordsCustomizer.CustomizeWord(word);

            if (!customizedWord.IsSuccess)
                return new Result<None>();
            
            if (!wordsRegister.ContainsKey(customizedWord.Value))
                wordsRegister.Add(customizedWord.Value, 1);
            else
                wordsRegister[customizedWord.Value]++;

            return new Result<None>();
        }

        public void AddRange(IEnumerable<string> words)
        {
            foreach (var word in words)
                Add(word);
        }

        public IOrderedEnumerable<Word> ToIOrderedEnumerable()
        {
            return wordsRegister
                .Select(e => new Word(e.Key, e.Value))
                .OrderByDescending(e => e.Count);
        }
    }
}