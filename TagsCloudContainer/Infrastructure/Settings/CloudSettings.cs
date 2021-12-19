using TagsCloudContainer.Spirals;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainer.Infrastructure.Settings
{
    public class CloudSettings
    {
        public ITagPainter Painter { get; set; }
        public ISpiral Spiral { get; set; }
    }
}
