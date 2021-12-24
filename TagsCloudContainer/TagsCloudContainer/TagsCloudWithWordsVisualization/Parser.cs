using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public static class Parser
    {
        public static Color ParseColor(string input)
        {
            return Color.FromName(input ?? string.Empty);
        }
        
        public static List<Color> ParseColors(string input)
        {
            return input.Split(' ').Select(ParseColor).ToList();    
        }
        
        public static List<Brush> ParseBrushes(string input)
        {
            return ParseColors(input)
                .Select(color => (Brush) new SolidBrush(color))
                .ToList();    
        }
        
        public  static Result<Point> ParsePoint(string input)
        {
            var coordinates = input.Split(' ').Select(int.Parse).ToList();
            return coordinates.Count() != 2 ? Result.Fail<Point>("Point must contain only 2 coordinates and can't be null") : Result.Ok(new Point(coordinates[0], coordinates[1]));
        }
    }
}