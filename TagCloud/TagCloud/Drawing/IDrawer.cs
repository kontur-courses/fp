using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.PreLayout;

namespace TagCloud.Drawing
{
    public interface IDrawer
    {
        Result<Bitmap> Draw(IDrawerOptions options, List<Result<Word>> words);
    }
}