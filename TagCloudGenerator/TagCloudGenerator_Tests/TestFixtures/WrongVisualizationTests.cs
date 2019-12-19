using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudGenerator.GeneratorCore.CloudLayouters;
using TagCloudGenerator.ResultPattern;
using TagCloudGenerator_Tests.WrongVisualization;

namespace TagCloudGenerator_Tests.TestFixtures
{
    [TestFixture]
    public class WrongVisualizationTests
    {
        private WrongVisualizationCloud wrongVisualizationCloud;
        private readonly List<string> createdTempFileNames = new List<string>();

        private static Size VisualizationImageSize => new Size(1000, 800);

        [TearDown]
        public void TearDown()
        {
            string failedTestsDirectoryName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed &&
                wrongVisualizationCloud != null)
            {
                var pathResult = WrongVisualizationSaver.SaveAndGetPathToWrongVisualization(
                    wrongVisualizationCloud, VisualizationImageSize, failedTestsDirectoryName);

                TestsHelper.HandleErrors(HandleError, pathResult);

                createdTempFileNames.Add(pathResult.Value);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            foreach (var fileName in createdTempFileNames)
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception exception) when (exception is IOException ||
                                                  exception is UnauthorizedAccessException) { }
        }

        [Test]
        [Order(1)]
        public void AlwaysFailedTest()
        {
            var imageCenter = new Point(VisualizationImageSize.Width / 2,
                                        VisualizationImageSize.Height / 2);
            var cloudLayouter = new CircularCloudLayouter(imageCenter);

            var randomizer = TestContext.CurrentContext.Random;
            var rectangleResults = Enumerable.Range(0, 50)
                .Select(i => cloudLayouter.PutNextRectangle(
                            new Size(randomizer.Next(50, 100), randomizer.Next(50, 100))))
                .Append(new Rectangle(imageCenter.X + 45, imageCenter.Y + 90, 86, 54).AsResult());

            var rectangles = TestsHelper.SelectValues(rectangleResults).ToArray();

            var intersectingRectangles = TestsHelper.GetAnyPairOfIntersectingRectangles(rectangles);

            if (intersectingRectangles.HasValue)
                wrongVisualizationCloud = new WrongVisualizationCloud(TestsHelper.BackgroundColor,
                                                                      TestsHelper.TagStyleByTagType,
                                                                      intersectingRectangles.Value, rectangles);

            Assert.Fail("Should fail to test logging functionality.");
        }

        [Test]
        [Order(2)]
        public void SaveWrongVisualization_WhenTestFailed_SavesWrongVisualization() =>
            File.Exists(createdTempFileNames.LastOrDefault());

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void WrongVisualizationCloudConstructor_OnEmptySizeWrongRectangles_ThrowArgumentException() =>
            Assert.Throws<ArgumentException>(() => new WrongVisualizationCloud(TestsHelper.BackgroundColor,
                                                                               TestsHelper.TagStyleByTagType,
                                                                               (Rectangle.Empty, Rectangle.Empty)));

        private static void HandleError(string error)
        {
            TestContext.Error.WriteLine(error);
            Environment.Exit(0);
        }
    }
}