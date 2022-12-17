using System;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloud;
using TagsCloud.Validators;

namespace TagsCloudTests
{
    [TestFixture]
    public class TagCloudShould
    {
        private TagCloud tagCloud;

        private readonly string textPath = @"..\..\..\..\text.txt";
        private readonly string picPath = @"..\..\..\..\testPicture";
        private readonly string extension = @".png";

        private ServiceProvider serviceProvider;

        [SetUp]
        public void SetUp()
        {
            serviceProvider = ContainerBuilder.GetNewTagCloudServices(1024, 720);
            tagCloud = serviceProvider.GetService<TagCloud>();
        }

        [Test]
        public void TagCloud_CommonInput_ShouldCreateFile()
        {
            var fullPath = picPath + extension;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            tagCloud.PrintTagCloud(textPath, picPath, extension);

            File.Exists(fullPath).Should().BeTrue();
        }

        [Test]
        public void TagCloud_NullTextFile_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(
                () => tagCloud.PrintTagCloud(
                    String.Empty, 
                    picPath, 
                    picPath));
        }

        [Test]
        public void TagCloud_NotCreatedTextFile_ShouldThrowExeption()
        {
            var path = @"..\..\..\..\testText.txt";

            if (File.Exists(path)) File.Delete(path);

            Assert.Throws<InvalidOperationException>(
                () => tagCloud.PrintTagCloud(
                    path, 
                    picPath, 
                    extension));
        }

        [Test]
        public void TagCloud_WordsDontFitOnCanvas_ShouldThrowExeption()
        {
            var path = @"..\..\..\..\text.txt";

            serviceProvider = ContainerBuilder.GetNewTagCloudServices(100, 100);
            tagCloud = serviceProvider.GetService<TagCloud>();

            Assert.Throws<InvalidOperationException>(
                () => tagCloud.PrintTagCloud(
                    path, 
                    picPath, 
                    extension));
        }

        [Test]
        public void PringSettings_NotInstalledFont_ShouldThrowExeption()
        {
            var validator = new SettingsValidator();
            var settings = new PrintSettings(validator);

            Assert.Throws<InvalidOperationException>(() => settings.SetFont("Super mega font", 64));
        }

        [Test]
        public void PringSettings_NegativeSizeOfPicture_ShouldThrowExeption()
        {
            var validator = new SettingsValidator();
            var settings = new PrintSettings(validator);

            Assert.Throws<InvalidOperationException>(() => settings.SetPictureSize(-10, -10));
        }
    }
}