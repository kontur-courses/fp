using System.Drawing;
using TagCloudResult;

namespace TagsCloudVisualization.Styling
{
    public class FontProperties
    {
        public string Name { get; }
        public int MinSize { get; }
        
        public FontProperties(string name, int minSize)
        {
            Name = name;
            MinSize = minSize;
        }

        public Result<Font> CreateFont(float size)
        {
            if(size<0)
                return Result.Fail<Font>($"Font size must be positive");
            
            using (var font = new Font(Name, 1))
            {
                if (font.Name != Name)
                    return Result.Fail<Font>($"Can't find font {Name}");
            }
            
            return Result.Ok(new Font(Name, size));
        }

    }
}