using System.Collections.Generic;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface IMorphologyAnalyzer
    {
        public Result<IEnumerable<ILexeme>> GetLexemesFrom(string filePath);
    }
}