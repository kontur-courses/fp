using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public class CloudVisualizationWord
    {
        public Rectangle Rectangle { get; }
        public string Word { get; }

        public CloudVisualizationWord(Rectangle rectangle, string word)
        {
            Rectangle = rectangle;
            Word = word;
        }
    }
}