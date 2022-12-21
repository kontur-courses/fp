using System.Collections.Generic;

namespace TagCloud.WordPreprocessors
{
    public interface IWordPreprocessor
    {
        public Result<IEnumerable<string>> GetPreprocessedWords();
    }
}
