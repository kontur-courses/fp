using System.Drawing;

namespace TagsCloudVisualization.UI
{
    public interface IVisualizer
    {
        void Start(string[] args);
        bool TryGetTagCloud(out Bitmap tagCloud);
        void InformUser(string error);
    }
}