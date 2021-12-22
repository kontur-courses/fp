using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualizationDI;
using TagsCloudVisualizationDI.Saving;
using TagsCloudVisualizationDI.TextAnalyze.Visualization;

namespace TagsCloudVisualizationDITests
{
    [TestFixture]
    public class VisualisationTests
    {
        [Test]
        public void ShouldNotThrowExcOnVisualisation()
        {
            var savePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\image";
            var saver = new DefaultSaver(savePath, ImageFormat.Png);
            var visualization = new DefaultVisualization(new Font("times", 15), new SolidBrush(Color.AliceBlue),
                new Size(10, 10), 10);
            Action act = () => visualization.DrawAndSaveImage(new List<RectangleWithWord>(), saver.GetSavePath().GetValueOrThrow(), ImageFormat.Png).GetValueOrThrow();
            act.Should().NotThrow();
        }

        [Test]
        public void ShouldThrowWneInvalidPath()
        {
            var visualization = new DefaultVisualization(new Font("times", 15), new SolidBrush(Color.AliceBlue),
                new Size(10, 10), 10);
            Action act = () => visualization.DrawAndSaveImage(new List<RectangleWithWord>(), "G:\\", ImageFormat.Png).GetValueOrThrow();
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ShouldCorrectlyCreateImage()
        {
            var savePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\image";
            var saver = new DefaultSaver(savePath, ImageFormat.Png);
            var visualization =
                new DefaultVisualization(new Font("times", 15), new SolidBrush(Color.AliceBlue),
                    new Size(10, 10), 10);
            visualization.DrawAndSaveImage(new List<RectangleWithWord>(), saver.GetSavePath().GetValueOrThrow(), ImageFormat.Png);
            File.Exists(saver.GetSavePath().GetValueOrThrow()).Should().BeTrue();
        }
    }
}
