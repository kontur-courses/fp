using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;
using TagCloudGenerator;
using TagCloudGenerator.TextProcessors;
using TagCloudGenerator.TextReaders;
using TagsCloudVisualization.PointDistributors;
using System.Drawing;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class ResultTests
    {
        private TagCloudDrawer tagCloudDrawer;
        private VisualizingSettings visualizingSettings;
        private readonly string toDicPath = "../../../TestsData/English (American).dic";
        private readonly string toAffPath = "../../../TestsData/English (American).aff";

        [SetUp]
        public void Setup()
        {
            var wordCounter = new WordCounter();
            var textReaders = new[] { (ITextReader)new TxtReader(), new PdfReader(), new DocxReader() };
            var textProcessors = new[] { (ITextProcessor)new BoringWordsTextProcessor(toDicPath, toAffPath), new WordsLowerTextProcessor() };
            visualizingSettings = new VisualizingSettings();
            tagCloudDrawer = new TagCloudDrawer(wordCounter, textProcessors, textReaders);
        }

        private static TestCaseData[] PathArguments =
        {
            new TestCaseData("../../../TestsData/testNotFound.docx").Returns("Could not find file ../../../TestsData/testNotFound.docx").SetName("ErrorWithDocxFormat"),
            new TestCaseData("../../../TestsData/testNotFound.txt").Returns("Could not find file ../../../TestsData/testNotFound.txt").SetName("ErrorWitTxtFormat"),
            new TestCaseData("../../../TestsData/testNotFound.pdf").Returns("Could not find file ../../../TestsData/testNotFound.pdf").SetName("ErrorWitPdfFormat"),
            new TestCaseData(null).Returns("There is no path to the file").SetName("WhenFilePathIsNull"),
            new TestCaseData("../../../TestsData/EmptyFile.txt").Returns("The file is empty").SetName("WhenFileIsEmpty"),
            new TestCaseData("../../../TestsData/test.rtf").Returns("File contains an unsuitable format for reading - .rtf").SetName("WhenPathUncorrectedFileFormat")
        };

        [TestOf(nameof(TagCloudDrawer))]
        [TestCaseSource(nameof(PathArguments))]
        public string WhenFilePathUncorrected_ShouldReturnCorrectErrorText(string filePath)
            => tagCloudDrawer.DrawWordsCloud(filePath, visualizingSettings).Error;

        [Test]
        [TestOf(nameof(TagCloudDrawer))]
        public void WhenPassWrongWayToDictionaries_ShouldReturnCorrectErrorText()
        {
            var processor = new BoringWordsTextProcessor("../../../Dictionaries/Uncorrected.dic",
                "../../../Dictionaries/Uncorrected.aff");

            var text = processor.ProcessText(new[] { "test", "tags" });

            text.Error.Should().Be("The path to the dictionary is incorrect or the dictionary is unsuitable");
        }

        [Test]
        [TestOf(nameof(TagCloudDrawer))]
        public void WhenPassUncorrectedVisualizingSettings_ShouldReturnCorrectErrorText()
        {
            visualizingSettings.ImageName = "test.txt";

            var image = tagCloudDrawer.DrawWordsCloud("../../../TestsData/test7.txt", visualizingSettings);
            var savingImage = tagCloudDrawer.SaveImage(image.Value, visualizingSettings);

            savingImage.Error.Should().Be("Uncorrected Image Format");
        }

        [Test]
        [TestOf(nameof(CircularCloudLayouter))]
        public void WhenPassUncorrectedRectangleSize_ShouldReturnCorrectErrorText()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Spiral(new Point(0,0), 1, 0.1));
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(0,0));

            rectangle.Error.Should().Be("Incorrect rectangle size");
        }
    }
}