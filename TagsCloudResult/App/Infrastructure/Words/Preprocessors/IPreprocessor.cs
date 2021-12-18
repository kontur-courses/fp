using System.Collections.Generic;

namespace App.Infrastructure.Words.Preprocessors
{
    public interface IPreprocessor
    {
        Result<IEnumerable<string>> Preprocess(IEnumerable<string> words);
    }
}