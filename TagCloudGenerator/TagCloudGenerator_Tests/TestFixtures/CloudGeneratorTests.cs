using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Clients.VocabularyParsers;
using TagCloudGenerator.GeneratorCore;
using TagCloudGenerator.GeneratorCore.CloudVocabularyPreprocessors;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator_Tests.TestFixtures
{
    [TestFixture]
    public class CloudGeneratorTests
    {
        private CloudOptions<CommonWordsCloud> cloudOptions;
        private ICloudVocabularyParser vocabularyParser;
        private ICloudContextGenerator contextGenerator;
        private CloudGenerator cloudGenerator;
        private string[] randomWords;
        private IEnumerable<string> excludedWords;

        private static CloudOptions<CommonWordsCloud> DefaultCloudOptions =>
            new CloudOptions<CommonWordsCloud>
            {
                CloudVocabularyFilename = "CommonWordsCloudFilename.txt",
                ImageSize = "800x600",
                ExcludedWordsVocabularyFilename = "ExcludedWords.txt",
                ImageFilename = "CommonWordsTagCloud.png",
                GroupsCount = 2,
                MutualFont = "Bahnschrift SemiLight",
                BackgroundColor = "#FFFFFFFF",
                FontSizes = "30_18",
                TagColors = "#FF000000_#FF000000"
            };

        [SetUp]
        public void SetUp()
        {
            cloudOptions = DefaultCloudOptions;

            randomWords = Enumerable.Range(0, 50)
                .Select(wordNumber => TestsHelper.GetRandomString((wordNumber / 10 + 1) * 2,
                                                                  TestContext.CurrentContext.Random))
                .ToArray();
            excludedWords = randomWords.Where(word => word.Length <= 2);

            vocabularyParser = A.Fake<ICloudVocabularyParser>();

            A.CallTo(() => vocabularyParser.GetCloudVocabulary(DefaultCloudOptions.CloudVocabularyFilename))
                .Returns(randomWords.AsEnumerable().AsResult());
            A.CallTo(() => vocabularyParser.GetCloudVocabulary(
                         DefaultCloudOptions.ExcludedWordsVocabularyFilename))
                .Returns(excludedWords.AsEnumerable().AsResult());

            contextGenerator = new CloudContextGenerator(cloudOptions, vocabularyParser);
            cloudGenerator = new CloudGenerator(contextGenerator, PreprocessorConstructor);
        }

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsContext() =>
            contextGenerator.GetTagCloudContext().IsSuccess.Should().BeTrue();

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectImageSize() =>
            contextGenerator.GetTagCloudContext().Value.ImageSize.Should().Be(new Size(800, 600));

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectImageName() =>
            contextGenerator.GetTagCloudContext().Value.ImageName
                .Should().Be(DefaultCloudOptions.ImageFilename);

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectTagCloudContent() =>
            contextGenerator.GetTagCloudContext().Value.TagCloudContent.Should().BeEquivalentTo(randomWords);

        [Test]
        public void GetTagCloudContext_OnDefaultOptionsForDoubleFontsCloud_ReturnsCorrectExcludedWords() =>
            contextGenerator.GetTagCloudContext().Value.ExcludedWords.Should().BeEquivalentTo(excludedWords);

        [Test]
        public void CreateTagCloudImage_OnDefaultOptionsForDoubleFontsCloud_CreateCloud() =>
            new CloudGenerator(contextGenerator, PreprocessorConstructor)
                .CreateTagCloudImage().IsSuccess.Should().BeTrue();

        [Test]
        public void CreateTagCloudImage_OnTooLargeFontSizes_ReturnsOutOfCanvasError()
        {
            cloudOptions.FontSizes = "500_100";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("out of canvas");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFontName_ReturnsNotInstalledError()
        {
            cloudOptions.MutualFont = "Wrong font";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("not installed on the current machine");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidImageSizeFormat_ReturnsSizeFormatError()
        {
            cloudOptions.ImageSize = "1080/1920";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("Invalid image size format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFilenameFormat_ReturnsFilenameFormatError()
        {
            cloudOptions.ImageFilename = "imageFilename|txt";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("Filename contains invalid character");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidFontSizesFormat_ReturnsFormatError()
        {
            cloudOptions.FontSizes = "20-32";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("Wrong format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidTagColorsFormat_ReturnsFilenameError()
        {
            cloudOptions.TagColors = "#F0A123BC_ABXYZ";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("Wrong format");
        }

        [Test]
        public void CreateTagCloudImage_OnInvalidTagBackgroundColorFormat_ReturnsFilenameError()
        {
            cloudOptions.BackgroundColor = "#BC0210";
            cloudGenerator.CreateTagCloudImage().Error.Should().Contain("Invalid background color");
        }

        private static CloudVocabularyPreprocessor PreprocessorConstructor(TagCloudContext cloudContext)
        {
            CloudVocabularyPreprocessor preprocessor = new ExcludingPreprocessor(null, cloudContext);
            preprocessor = new ToLowerPreprocessor(preprocessor);

            return preprocessor;
        }
    }
}