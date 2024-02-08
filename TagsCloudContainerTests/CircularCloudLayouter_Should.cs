using TagsCloudContainer.Algorithm;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Infrastucture.Settings;
using TagsCloudContainer.Infrastucture;
using FakeItEasy;
using System.Drawing;

namespace TagsCloudContainerTests
{

    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private AlgorithmSettings algSettings;
        private ImageSettings imgSettings;

        [SetUp]
        public void SetUp()
        {
            algSettings = new AlgorithmSettings();
            imgSettings = new ImageSettings();
            layouter = new CircularCloudLayouter(imgSettings,
                new RectanglePlacer(algSettings, imgSettings),
                new CloudSizer(imgSettings));
        }

        [Test]
        public void CreateAnEmptyLayout_WhenFrequencyDictionaryIsEmpty()
        {
            var textRectangles = layouter.GetRectangles(new Dictionary<string, int>());

            textRectangles.IsSuccess.Should().BeTrue();
            textRectangles.Value.Should().BeEmpty();
        }

        [Test]
        public void CreateLayoutWithoutIntersections_WhenNoIdenticalFrequencies()
        {
            var wordsFrequencies = new Dictionary<string, int>()
            {
                {"яблоко" , 1},
                {"банан", 2},
                {"груша", 6},
                {"мандарин", 17},
            };

            var textRectangles = layouter.GetRectangles(wordsFrequencies);

            textRectangles.IsSuccess.Should().BeTrue();
            IsIntersections(textRectangles.Value).Should().BeFalse();
        }

        [Test]
        public void CreateLayoutWithoutIntersections_WhenIdenticalFrequencies()
        {
            var wordsFrequencies = new Dictionary<string, int>()
            {
                {"яблоко" , 1},
                {"банан", 6},
                {"груша", 6},
                {"мандарин", 17},
                {"апельсин", 17},
            };

            var textRectangles = layouter.GetRectangles(wordsFrequencies);

            textRectangles.IsSuccess.Should().BeTrue();
            IsIntersections(textRectangles.Value).Should().BeFalse();
        }

        [Test]
        public void ReturnResultFail_WhenTagCloudGoBeyondBoundariesOfImage()
        {
            imgSettings.Height = 10;
            imgSettings.Width = 10;
            var rectanglePlacer = A.Fake<IRectanglePlacer>();
            var cloudSizer = A.Fake<ICloudSizer>();
            layouter = new CircularCloudLayouter(imgSettings, rectanglePlacer, cloudSizer);
            var testRectangle = new RectangleF(1, 1, 100, 100);
            var wordsFrequencies = new Dictionary<string, int>()
            {
                {"яблоко" , 1}
            };

            A.CallTo(() => rectanglePlacer.GetPossibleNextRectangle(A<List<TextRectangle>>._, A<SizeF>._))
                .Returns(Result.Ok(testRectangle));

            var textRectangles = layouter.GetRectangles(wordsFrequencies);

            textRectangles.IsSuccess.Should().BeFalse();
            textRectangles.Error.Should().Be("The tag cloud goes beyond the boundaries of the image");
        }

        public bool IsIntersections(List<TextRectangle> textRectangles)
        {
            var rectList = textRectangles.Select(x => x.Rectangle).ToList();

            for (int i = 0; i < rectList.Count; i++)
                for (int j = i + 1; j < rectList.Count; j++)
                    if (rectList[i].IntersectsWith(rectList[j]))
                        return true;

            return false;
        }

    }
}
