using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Layouter.PointsProviders;

namespace TagsCloudContainer_Tests
{
    [TestFixture]
    public class PointsProvidersResolver_Should
    {
        private readonly IPointsProvider pointsProvider = A.Fake<IPointsProvider>();
        private PointsProvidersResolver sut;

        [Test]
        public void ReturnsOkResult_WhenProviderExist()
        {
            A.CallTo(() => pointsProvider.AlghorithmName).Returns(LayoutAlrogorithm.Circular);
            sut = new PointsProvidersResolver(pointsProvider);
            sut.Get(LayoutAlrogorithm.Circular).IsSuccess
                .Should()
                .BeTrue();
        }

        [Test]
        public void ReturnsFailResultWithMessage_WhenProviderDoesNotExist()
        {
            A.CallTo(() => pointsProvider.AlghorithmName).Returns((LayoutAlrogorithm)int.MaxValue);
            sut = new PointsProvidersResolver(pointsProvider);
            sut.Get(LayoutAlrogorithm.Circular).Error
                .Should()
                .MatchRegex(@"does not exist");
        }
    }
}