using System;
using System.Collections.Generic;
using System.Text;

namespace TagsCloudContainer.App.Layouter
{
    public interface ITagsPainter
    {
        public void Paint(IEnumerable<TagInfo> tags);

        public bool CanPaint(PainterType painterType);
    }
}
