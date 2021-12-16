using System.Collections.Generic;

namespace App.Infrastructure.Words.Preprocessors
{
    public interface IPreprocessor
    {
        Result<IEnumerable<string>> Preprocess(Result<IEnumerable<string>> words);
    }
}