using System.Drawing;
using TagCloud.TagCloudVisualisation.Spirals;

namespace TagCloud.TagCloudVisualisation.TagCloudLayouter
{
    public class LayouterFactory
    {
        public ITagCloudLayouter GetCircularLayouter(Point center)
        {
            return new CircularCloudLayouter(center);
        }
        
        public ITagCloudLayouter GetCircularLayouter(Point center, ISpiral spiral)
        {
            return new CircularCloudLayouter(center, spiral);
        }
    }
}