using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Clients.VocabularyParsers;
using TagCloudGenerator.GeneratorCore;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator_Tests.TestFixtures
{
    [TestFixture]
    public class CloudGeneratorTests
    {
        private ITagCloudOptions<ITagCloud> cloudOptions;
        private ICloudVocabularyParser vocabularyParser;
        private ICloudContextGenerator contextGenerator;
        private string[] randomWords;
        private IEnumerable<string> excludedWords;

        [SetUp]
        public void SetUp()
        {
            cloudOptions = TestsHelper.DefaultCloudOptions;

            randomWords = Enumerable.Range(0, 50)
                .Select(wordNumber => TestsHelper.GetRandomString((wordNumber / 10 + 1) * 2,
                                                                  TestContext.CurrentContext.Random))
                .ToArray();
            excludedWords = randomWords.Where(word => word.Length <= 2);

            vocabularyParser = A.Fake<ICloudVocabularyParser>();

            A.CallTo(() => vocabularyParser.GetCloudVocabulary(TestsHelper.DefaultCloudOptions.CloudVocabularyFilename))
                .Returns(randomWords.AsEnumerable().AsResult());
            A.CallTo(() => vocabularyParser.GetCloudVocabulary(
                         TestsHelper.DefaultCloudOptions.ExcludedWordsVocabularyFilename))
                .Returns(excludedWords.AsEnumerable().AsResult());

            contextGenerator = new CloudContextGenerator(cloudOptions, vocabularyParser);
        }

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsContext() =>
            contextGenerator.GetTagCloudContext().IsSuccess.Should().BeTrue();

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectImageSize() =>
            contextGenerator.GetTagCloudContext().Value.ImageSize.Should().Be(new Size(800, 600));

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectImageName() =>
            contextGenerator.GetTagCloudContext().Value.ImageName.Should()
                .Be(TestsHelper.DefaultCloudOptions.ImageFilename);

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectTagCloudContent() =>
            contextGenerator.GetTagCloudContext().Value.TagCloudContent.Should().BeEquivalentTo(randomWords);

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectExcludedWords() =>
            contextGenerator.GetTagCloudContext().Value.ExcludedWords.Should().BeEquivalentTo(excludedWords);

        [Test]
        public void CreateTagCloudImage_OnDefaultOptionsForDoubleFontsCloud_CreateCloud() =>
            new CloudGenerator(contextGenerator, TestsHelper.PreprocessorConstructor)
                .CreateTagCloudImage().IsSuccess.Should().BeTrue();

        [Test]
        public void CreateTagCloudImage_OnTooLargeFontSizes_ReturnsOutOfCanvasError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).FontSizes = "500_100";
            ErrorShouldContain("out of canvas");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFontName_ReturnsNotInstalledError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).MutualFont = "Wrong font";
            ErrorShouldContain("not installed on the current machine");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidImageSizeFormat_ReturnsSizeFormatError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).ImageSize = "1080/1920";
            ErrorShouldContain("Invalid image size format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFilenameFormat_ReturnsFilenameFormatError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).ImageFilename = "imageFilename|txt";
            ErrorShouldContain("Filename contains invalid character");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFontSizesFormat_ReturnsFormatError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).FontSizes = "20-32";
            ErrorShouldContain("Wrong format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidTagColorsFormat_ReturnsFilenameError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).TagColors = "#F0A123BC_ABXYZ";
            ErrorShouldContain("Wrong format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidTagBackgroundColorFormat_ReturnsFilenameError()
        {
            ((CloudOptions<CommonWordsCloud>)cloudOptions).BackgroundColor = "#BC0210";
            ErrorShouldContain("Invalid background color");
        }

        private void ErrorShouldContain(string errorMessage) =>
            new CloudGenerator(contextGenerator, TestsHelper.PreprocessorConstructor)
                .CreateTagCloudImage().Error.ToLower().Should().Contain(errorMessage.ToLower());
    }
}