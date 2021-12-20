using System.Drawing;
using TagsCloud.Visualization.FontFactory;

namespace TagsCloud.Visualization.Models
{
    public record Tag(Word Word, FontDecorator FontDecorator, Rectangle Border);
}