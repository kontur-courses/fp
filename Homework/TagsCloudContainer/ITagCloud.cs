using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface ITagCloud
    {
        public Result<Bitmap> LayDown(IEnumerable<string> words);
    }
}