using System.Drawing;
using TagsCloudVisualization.Infrastructure.TextAnalysing;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITagColorChooser
    {
        Color GetTokenColor(Token token);
    }
}