using System.Drawing;

public interface IColorProvider
{
    Result<Color> GetColor(WordLayout layout);
}
