using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.Settings;

namespace TagsCloudContainerTests
{
    public class TagCloudDrawerTests
    {
        private IAppSettings settings;
        private TagCloudDrawer tagCloudDrawer;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            settings = new AppSettings()
            {
                ImageWidth = 1000,
                ImageHeight = 1000,
                BackgroundColorName = "White",
                FontColorName = "Black"
            };

            tagCloudDrawer = new TagCloudDrawer(settings);
        }
        
        [Test]
        public void DrawImage_ReturnsFailResult_WhenTagCollectionIsEmpty()
        {
            var result = tagCloudDrawer.DrawImage(new List<Tag>());

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Tag cloud doesn't contain any tags");
        }

        [Test]
        public void DrawImage_ReturnsFailResult_WhenImageSizeIsInvalid()
        {
            var settings = new AppSettings()
            {
                ImageWidth = -100
            };
            var drawer = new TagCloudDrawer(settings);

            var result = drawer.DrawImage(new List<Tag>());

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Image width and image height must be greater than zero");
        }
        
        [Test]
        public void DrawImage_CreatesImageWithSizeFromSettings()
        {
            var randomTag = new Tag("asd", new Rectangle(Point.Empty, new Size(10, 10)), new Font("Arial", 10));
            
            var image = tagCloudDrawer.DrawImage(new List<Tag>() { randomTag }).GetValueOrThrow();

            image.Size.Height.Should().Be(settings.ImageHeight);
            image.Size.Width.Should().Be(settings.ImageWidth);
        }
    }
}