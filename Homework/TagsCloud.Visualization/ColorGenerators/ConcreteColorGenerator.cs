using System.Drawing;

namespace TagsCloud.Visualization.ColorGenerators
{
    public class ConcreteColorGenerator : IColorGenerator
    {
        private readonly Color color;
        public ConcreteColorGenerator(Color color) => this.color = color;

        public Color Generate() => color;
    }
}