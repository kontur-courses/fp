using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public class CloudVisualizationWord
    {
        public CloudVisualizationWord(Rectangle rectangle, string word)
        {
            Rectangle = rectangle;
            Word = word;
        }

        public Rectangle Rectangle { get; }
        public string Word { get; }
    }
}