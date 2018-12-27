using NUnit.Framework;
using System;
using FluentAssertions;
using System.Drawing;

namespace TagsCloudContainer.Util
{
    [TestFixture]
    public class AutofacConfig_Should
    {
        private string baseDomain = $"{AppDomain.CurrentDomain.BaseDirectory}/";
        [Test]
        public void Constructor_CorrectInputAndOutput()
        {
            var args = new string[] { "-i", baseDomain + "input/input.txt", "-o", baseDomain + "output/o.png" };
            var container = new AutofacContainer(args);

            container.TagCloud.Tags.Value.Length.Should().Be(55);
        }

        [Test]
        public void Constructor_CorrectExcludeBoringWords()
        {
            var args = new string[] { "-i", baseDomain + "input/input.txt", "-o", baseDomain + "output/o.png", "--words-to-exclude", baseDomain + "input/words to exclude.txt" };

            var container = new AutofacContainer(args);
            
            container.TagCloud.Tags.Value.Length.Should().Be(51);
        }

        [Test]
        public void Constructor_Correct()
        {
            var args = new string[] { "-i", baseDomain + "input/input.txt", "-o", baseDomain + "output/o.png", "-c", "red",
                "--words-to-exclude", baseDomain + "input/words to exclude.txt", "-f", "Arial" };

            var container = new AutofacContainer(args);            

            container.Brush.Value.Should().Be(Brushes.Red);
            container.FontName.Value.Should().Be("Arial");
            container.OutputPath.Value.Should().Be(baseDomain + "output/o.png");

        }
    }

}
