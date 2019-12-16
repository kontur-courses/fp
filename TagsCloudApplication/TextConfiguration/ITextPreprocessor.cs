using System.Collections.Generic;

namespace TextConfiguration
{
    public interface ITextPreprocessor
    {
        Result<List<string>> PreprocessText(string text);
    }
}
