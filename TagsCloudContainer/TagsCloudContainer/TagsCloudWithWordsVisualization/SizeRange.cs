using System.Drawing;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public class SizeRange
    {
        public readonly Size MinSize;
        public readonly Size MaxSize;

        public SizeRange(Size minSize, Size maxSize)
        {
            MinSize = minSize;
            MaxSize = maxSize;
        }
    }
}