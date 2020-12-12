using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.CloudGenerator;
using TagsCloudContainer.App.CloudVisualizer;
using TagsCloudContainer.App.Settings;

namespace TagsCloudContainerTests
{
    internal class CloudPainterTests
    {
        private readonly ImageSizeSettings imageSizeSettings;
        private readonly FontSettings fontSettings;
        private readonly CloudPainter cloudPainter;

        public CloudPainterTests()
        {
            imageSizeSettings = ImageSizeSettings.Instance;
            fontSettings = FontSettings.Instance;
            cloudPainter = new CloudPainter(Palette.Instance, fontSettings, imageSizeSettings);
        }

        [Test]
        public void CloudPainter_ShouldPaintSuccessfully_IfTagsAreNotInOutImageBounds()
        {
            imageSizeSettings.Width = 100;
            imageSizeSettings.Height = 100;
            fontSettings.Font = new Font("Arial", 10);
            var cloud = new[] {new Tag("word", fontSettings.Font.Size, 
                new Rectangle(new Point(0, 0), TextRenderer.MeasureText("word", fontSettings.Font)))};
            cloudPainter
                .Paint(cloud, Graphics.FromImage(new Bitmap(imageSizeSettings.Width, imageSizeSettings.Height)))
                .IsSuccess
                .Should()
                .BeTrue();
        }

        [Test]
        public void CloudPainter_ShouldReturnError_IfTagsAreInOutImageBounds()
        {
            imageSizeSettings.Width = 10;
            imageSizeSettings.Height = 10;
            fontSettings.Font = new Font("Arial", 50);
            var cloud = new[] {new Tag("word", fontSettings.Font.Size,
                new Rectangle(new Point(0, 0), TextRenderer.MeasureText("word", fontSettings.Font)))};
            cloudPainter
                .Paint(cloud, Graphics.FromImage(new Bitmap(imageSizeSettings.Width, imageSizeSettings.Height)))
                .Error
                .Should()
                .BeEquivalentTo("Tag is out of image bounds");
        }
    }
}
