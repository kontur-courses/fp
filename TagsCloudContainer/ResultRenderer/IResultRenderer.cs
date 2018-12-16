using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.ResultRenderer
{
    public interface IResultRenderer
    {
        Result<Image> Generate(IEnumerable<Word> words);

        IResultRenderer WithConfig(IResultRendererConfig config);
    }
}