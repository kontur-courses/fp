using System.Drawing;
using TagsCloudContainer.Core.Layouter;

namespace TagsCloudContainer.Core.Drawer.Interfaces
{
    public interface ILayoutDrawer
    {
        public void AddTags(IEnumerable<Tag> tags);

        public void Draw(Graphics graphics);
    }
}
