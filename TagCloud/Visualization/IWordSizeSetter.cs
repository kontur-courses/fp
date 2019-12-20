using System.Collections.Generic;
using ResultOf;
using TagCloud.Infrastructure;

namespace TagCloud.Visualization
{
    public interface IWordSizeSetter
    {
        Result<IEnumerable<Word>> GetSizedWords(IEnumerable<Word> words, PictureConfig pictureConfig);
    }
}
