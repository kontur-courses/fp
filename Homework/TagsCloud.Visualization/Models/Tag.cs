using System.Drawing;
using TagsCloud.Visualization.FontFactories;

namespace TagsCloud.Visualization.Models
{
    public record Tag(Word Word, FontDecorator FontDecorator, Rectangle Border);
}