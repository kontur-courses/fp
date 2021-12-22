using System.Drawing;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.TextAnalysing;

namespace TagsCloudVisualization.Default
{
    public class SingleTagColor : ITagColorChooser
    {
        private readonly Color color;

        public SingleTagColor(Color color)
        {
            this.color = color;
        }

        public Color GetTokenColor(Token token)
        {
            return color;
        }
    }
}