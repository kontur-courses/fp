using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudVisualization;
using TagsCloudContainer.TagsCloudWithWordsVisualization;

namespace TagsCloudContainer.Tests
{
    public class VisualizerTests
    {
        [Test]
        public void GetCloudVisualization_Fails_WhenWordsIsNull()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(null,
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Words can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenTagsColorsAreNull()
        {
            var parameters = new VisualizationParameters(null, Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Tags colors can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinHeightIsGreaterThanMaxHeight()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(50, 500), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters)
                .Error.Should()
                .Be("Min tag size must be less or equal than max tag size");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinWidthIsGreaterThanMaxWidth()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(500, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters)
                .Error.Should()
                .Be("Min tag size must be less or equal than max tag size");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenLayouterIsNull()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(), null, 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Layouter can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenCoefficientIsNotPositive()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), -0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters)
                .Error.Should()
                .Be("Reduction coefficient must be between 0 and 1");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenCoefficientIsGreaterOrEqualOne()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 1.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters)
                .Error.Should()
                .Be("Reduction coefficient must be between 0 and 1");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMinFontSizeIsNotPositive()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, -1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>() {"a"},
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Font size can't be zero or negative");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenMaxTagSizeIsNotPositive()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(-500, 50), new Size(-100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>() {"a"},
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Width and height can't be negative");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenNotHaveEnoughSpace()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(2000, 2000), new Size(2000, 2000)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>() {"a"},
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters)
                .Error.Should()
                .Be("Don't have enough space to put next rectangle");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenFontFamilyIsNull()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), null, 1, new List<Brush>() {Brushes.Aqua});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Font family can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenBrushesAreNull()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1, null);
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Brushes can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenOneOfBrushesIsNull()
        {
            var parameters = new VisualizationParameters(new List<Color>(), Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 1,
                new List<Brush>() {null});
            var cloudParameters = new TagCloudParameters(new List<string>(),
                new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Brush can't be null");
        }

        [Test]
        public void GetCloudVisualization_Fails_WhenWordIsTooLong()
        {
            var parameters = new VisualizationParameters(new List<Color>() {Color.Aqua}, Color.Aqua,
                new SizeRange(new Size(50, 50), new Size(100, 50)), FontFamily.GenericSansSerif, 5,
                new List<Brush>() {Brushes.Aqua});
            var cloudParameters =
                new TagCloudParameters(
                    new List<string>() {"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"},
                    new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500))), 0.8, parameters, null);
            Visualizer.GetCloudVisualization(cloudParameters).Error.Should().Be("Word is too long for tag cloud");
        }
    }
}