using System.Collections.Generic;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public interface IWordExcluder
    {
        Result<HashSet<string>> GetExcludedWords();
        Result<None> SetExcludedWord(string word);
    }
}