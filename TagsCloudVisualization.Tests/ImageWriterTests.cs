using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.ImageWriters;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ImageWriterTests
    {
        private ImageWriter writer;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            writer = new ImageWriter();
        }

        [TestCase("test.bmp")]
        [TestCase("test.gif")]
        [TestCase("test.ico")]
        [TestCase("test.jpg")]
        [TestCase("test.jpeg")]
        [TestCase("test.png")]
        [TestCase("test.tif")]
        [TestCase("test.tiff")]
        [TestCase("test.wmf")]
        public void Save_ShouldWorksCorrectly_WithSupportedImageFormats(string filename)
        {
            var actual = writer.Save(new Bitmap(1, 1), filename);

            actual.Should().Be(Result.Ok());
        }
        
        [TestCase("test.mp3")]
        [TestCase("test.doc")]
        public void Save_ShouldThrowArgumentException_WhenPassUnknownImageFormat(string filename)
        {
            var actual = writer.Save(new Bitmap(1, 1), filename);
            
            Console.WriteLine(actual.Error);
            actual.Should().BeEquivalentTo(Result.Fail<None>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<None>)));
        }
        
        [TestCase(":.png")]
        [TestCase("?.png")]
        public void Save_ShouldThrowException_WhenInvalidPath(string filename)
        {
            var actual = writer.Save(new Bitmap(1, 1), filename);
            
            Console.WriteLine(actual.Error);
            actual.Should().BeEquivalentTo(Result.Fail<None>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<None>)));
        }
    }
}