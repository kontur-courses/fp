using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudContainer
{
    public class VisualizerTests
    {
        [Test]
        public void GetCloudVisualization_Fails_WhenWordsIsNull()
        {
            Visualizer.GetCloudVisualization(null, new List<Color>() {Color.Aqua}, Color.Aqua, new Size(50, 50),
                    new Size(100, 50), new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8,
                    1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Words can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenTagsColorsAreNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), null, Color.Aqua, new Size(50, 50), new Size(100, 50),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Tags colors can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinHeightIsGreaterThanMaxHeight()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 500), new Size(100, 50),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Min tag size must be less or equal than max tag size");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinWidthIsGreaterThanMaxWidth()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(500, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Min tag size must be less or equal than max tag size");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenLayouterIsNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100), null, 0.8, 1, FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Layouter can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenCoefficientIsNotPositive()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Reduction coefficient must be between 0 and 1");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenCoefficientIsGreaterOrEqualOne()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 1, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Reduction coefficient must be between 0 and 1");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinFontSizeIsNotPositive()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.5, -1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Font size can't be zero or negative");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMaxTagSizeIsNotPositive()
        {
            Visualizer.GetCloudVisualization(new List<string>() {"a"}, new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(-500, -500), new Size(-100, -100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.5, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Width and height can't be negative");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenNotHaveEnoughSpace()
        {
            Visualizer.GetCloudVisualization(new List<string>() {"a"}, new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(2000, 2000),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.5, 1,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Don't have enough space to put next rectangle");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenFontFamilyIsNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.5, 1, null,
                    Brushes.Aqua)
                .Error.Should()
                .Be("Font family can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenBrushesAreNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.5, 1,
                    FontFamily.GenericSansSerif, null as List<Brush>)
                .Error.Should()
                .Be("Brushes can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenBrushIsNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 5,
                    FontFamily.GenericSansSerif, null as Brush)
                .Error.Should()
                .Be("Brush can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenOneOfBrushesIsNull()
        {
            Visualizer.GetCloudVisualization(new List<string>(), new List<Color>() {Color.Aqua}, Color.Aqua,
                    new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 5,
                    FontFamily.GenericSansSerif, new List<Brush>() {null})
                .Error.Should()
                .Be("Brush can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenWordIsTooLong()
        {
            Visualizer.GetCloudVisualization(new List<string>() {"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"},
                    new List<Color>() {Color.Aqua}, Color.Aqua, new Size(50, 50), new Size(100, 100),
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, 5,
                    FontFamily.GenericSansSerif, Brushes.Aqua)
                .Error.Should()
                .Be("Word is too long for tag cloud");
        }
    }
}