using System.Drawing;
using FluentAssertions;
using TagsCloud.Distributors;
using TagsCloud.Layouters;
using TagsCloud.Options;

namespace TagsCloudTests.Layouters;

public class CircularLayouterTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private LayouterOptions options;
        private CircularCloudLayouter tagsCloud;
        private SpiralDistributor distributor;

        private Dictionary<string, Font> simpleDictionary = new()
        {
            { "apple", new Font("Arial", 5) },
            { "pineapple", new Font("Arial", 3) }
        };

        [SetUp]
        public void SetUp()
        {
            options = new LayouterOptions() { Center = new Point() };
            distributor = new SpiralDistributor(options);
            tagsCloud = new CircularCloudLayouter(distributor, options);
        }

        [Test]
        public void CircularCloudLayouter_InitializeParams()
        {
            tagsCloud.Center.Should().BeEquivalentTo(options.Center);
        }

        [Test]
        public void CreateTagsCloud_ShouldPlaceFirstOnCenter()
        {
            tagsCloud.CreateTagsCloud(simpleDictionary)
                .GetValueOrThrow().Tags
                .ToArray()[0].TagRectangle.Location.Should().Be(options.Center);
        }

        [Test]
        public void CreateTagsCloud_ShouldCalculateCloudSize()
        {
            tagsCloud.CreateTagsCloud(simpleDictionary).GetValueOrThrow().CloudSize.Should()
                .BeEquivalentTo(new Size(20, 11));
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

        [Test]
        public void CreateTagsCloud_ShouldRuturnErrorResult_WhenInputDictionaryIsEmpty()
        {
            tagsCloud.CreateTagsCloud(new Dictionary<string, Font>() { }).IsSuccess.Should().BeFalse();
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