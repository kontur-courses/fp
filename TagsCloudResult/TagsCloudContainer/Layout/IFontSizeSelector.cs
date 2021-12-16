using System.Collections.Generic;

namespace TagsCloudContainer.Layout
{
    public interface IFontSizeSelector
    {
        IEnumerable<SizedWord> GetFontSizedWords(IEnumerable<string> words);
    }
}