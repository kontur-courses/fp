using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator_Tests.WrongVisualization
{
    public static class WrongVisualizationSaver
    {
        public static Result<string> SaveAndGetPathToWrongVisualization(
            TagCloud<TagType> tagCloud, Size imageSize, string directoryName)
        {
            var failedTestFilename = $"{GetCurrentTestName()}_{DateTime.Now:dd.MM.yyyy-HH.mm.ss}.png";

            Directory.CreateDirectory(directoryName);
            var wrongVisualisationImageFilepath = Path.Combine(
                TestContext.CurrentContext.TestDirectory, directoryName, failedTestFilename);

            var bitmapResult = tagCloud.CreateBitmap(null, null, imageSize);

            if (!bitmapResult.IsSuccess)
                return Result.Fail<string>(
                    $"Bitmap creation error was handled:{Environment.NewLine}{bitmapResult.Error}");

            bitmapResult.Value.Save(wrongVisualisationImageFilepath);

            TestContext.WriteLine($@"Tag cloud visualization saved to file:{Environment.NewLine
                                      }{wrongVisualisationImageFilepath}");

            return wrongVisualisationImageFilepath.AsResult();

            static string GetCurrentTestName() => TestContext.CurrentContext.Test is var test &&
                                                  test.MethodName == test.Name
                                                      ? test.Name
                                                      : test.MethodName + test.Name;
        }
    }
}