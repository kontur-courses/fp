using System.Drawing;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITokenColorChooser
    {
        Color GetTokenColor(Token token);
    }
}