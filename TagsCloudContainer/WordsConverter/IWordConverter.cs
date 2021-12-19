using System.Collections.Generic;

namespace TagsCloudContainer.WordsConverter
{
    public interface IWordConverter
    {
        Result<List<Tag>> ConvertWords(List<string> words);
    }
}