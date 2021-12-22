using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ICloudLayouter
    {
        public ICloud Cloud { get; }

        public void PutNextTag(SizeF tagLayouterSize, ITag tag);
    }
}