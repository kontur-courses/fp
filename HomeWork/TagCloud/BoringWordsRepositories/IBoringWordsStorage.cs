using System.Collections.Generic;

namespace TagCloud.BoringWordsRepositories
{
    public interface IBoringWordsStorage
    {
        public string FileExtFilter { get; }
        public void LoadBoringWords(string path);
        public Result<HashSet<string>> GetBoringWords();
    }
}
