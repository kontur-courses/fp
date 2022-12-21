using System.Collections.Generic;
using System.Linq;
using TagCloud.Readers;

namespace TagCloud.BoringWordsRepositories
{
    public class TextFileBoringWordsStorage : IBoringWordsStorage
    {
        private readonly IBoringWordsReader reader;
        private Result<HashSet<string>> boringWords;

        public TextFileBoringWordsStorage(IBoringWordsReader reader)
        {
            boringWords = new HashSet<string>();
            this.reader = reader;
        }

        public string FileExtFilter => reader.FileExtFilter;

        public void LoadBoringWords(string path)
        {
            boringWords.Value.Clear();
            reader.SetFile(path);
            boringWords = reader.ReadWords()
                .Then(words => words.Select(word => word.ToLower()).ToHashSet());
        }

        public Result<HashSet<string>> GetBoringWords() => boringWords;
    }
}
