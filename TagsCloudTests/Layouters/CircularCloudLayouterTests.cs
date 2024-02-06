using System.Drawing;
using FluentAssertions;
using TagsCloud.ConsoleCommands;
using TagsCloud.Distributors;
using TagsCloud.Layouters;
using TagsCloud.WordFontCalculators;

namespace TagsCloudTests.Layouters;

public class CircularLayouterTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [SetUp]
        public void SetUp()
        {
            distributor = new SpiralDistributor(center);
            tagsCloud = new CircularCloudLayouter(distributor);
        }

        private Point center;
        private CircularCloudLayouter tagsCloud;
        private SpiralDistributor distributor;

        [Test]
        public void CircularCloudLayouter_InitializeParams()
        {
            tagsCloud.Center.Should().Be(new Point());
        }


        [Test]
        public void PutNextRectangle_ShouldPlaceFirstOnCenter()
        {
            tagsCloud.CreateTagsCloud(new Dictionary<string, Font>() { { "Реваванат", new Font("Arial", 3) } })
                .GetValueOrThrow().Tags
                .ToArray()[0].TagRectangle.Location.Should().Be(center);
        }

        [Test]
        public void CircularCloudLayouter_ShouldHasNoIntersections_When1000Words()
        {
            var tags = tagsCloud.CreateTagsCloud(GetRandomWordsDictionary()).GetValueOrThrow().Tags;
            tags.Any(tag1 => tags.Any(tag2 =>
                    tag1.TagRectangle.IntersectsWith(tag2.TagRectangle) && tag1 != tag2))
                .Should().BeFalse();
        }

        [Test]
        public void CircularCloudLayouter_ShouldBeCloseToCircle()
        {
            var randomDict = GetRandomWordsDictionary();
            tagsCloud.CreateTagsCloud(randomDict).GetValueOrThrow().Tags
                .All(tag =>
                {
                    var distanceToCenter =
                        Math.Sqrt(Math.Pow(tag.TagRectangle.X - tagsCloud.Center.X, 2) +
                                  Math.Pow(tag.TagRectangle.Y - tagsCloud.Center.Y, 2));
                    return distanceToCenter <= distributor.Radius;
                }).Should().BeTrue();
        }


        private Dictionary<string, Font> GetRandomWordsDictionary()
        {
            var random = new Random();
            var dict = new Dictionary<string, Font>();
            for (var i = 0; i < 1000; i++)
            {
                dict[$"{i}"] = new Font("Arial", random.Next(1, 100));
            }

            return dict;
        }
    }
}