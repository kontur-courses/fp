using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Core;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud;
using TagsCloud.BoringWordsDetectors;
using TagsCloud.CloudRenderers;
using TagsCloud.ColorSelectors;
using TagsCloud.PointsLayouts;
using TagsCloud.StatisticProviders;
using TagsCloud.WordLayouters;
using TagsCloud.WordReaders;
using TagsCloud.WordSelectors;

namespace TagsCloudTests
{
    public class TagsCloudTests
    {
        private ContainerBuilder builder;
        private string directoryInfo;
        private string samplePath;
        
        [SetUp]
        public void SetUp()
        {
            builder = new ContainerBuilder();
            
            builder.RegisterType<AllWordSelector>().As<IWordSelector>();
            builder.RegisterType<ByCollectionBoringWordsDetector>().As<IBoringWordsDetector>();
            builder.RegisterType<StatisticProvider>().As<IStatisticProvider>();
            builder.RegisterInstance(new FontFamily("Arial"));
            builder.RegisterType<SpiralPoints>().As<IPointsLayout>();
            builder.RegisterType<WordLayouter>().SingleInstance().As<IWordLayouter>();
            builder.RegisterInstance(new[]
                {Color.Black, Color.Red, Color.Blue, Color.Green, Color.Yellow});
            builder.RegisterType<RandomColorSelector>().SingleInstance().As<IColorSelector>();

            directoryInfo = TestContext.CurrentContext.TestDirectory;
            samplePath = $"{directoryInfo}\\sample.png";
            new FileInfo(samplePath).Delete();
        }
        
        [Test]
        public void CreateCloud_FromTxt()
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));
            
            builder.RegisterType<CloudRenderer>()
                .As<ICloudRenderer>()
                .WithParameters(new Parameter[]
                {
                    new NamedParameter("width", 3000),
                    new NamedParameter("height", 3000), 
                    new NamedParameter("filePath", samplePath)
                });
            
            Program.MakeCloud(builder.Build());
            var actual = new FileInfo(samplePath);
            Assert.True(actual.Exists);
        }

        [Test]
        public void WriteError_WhenFileNotFound()
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\none.txt"));

            Program.MakeCloud(builder.Build()).Error.Should().ContainAll("Файл", "none.txt", "не найден");
        }

        [Test]
        public void WriteError_WhenNotPositiveWidthOrHeight()
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));
            
            builder.RegisterType<CloudRenderer>()
                .As<ICloudRenderer>()
                .WithParameters(new Parameter[]
                {
                    new NamedParameter("width", 0),
                    new NamedParameter("height", 3000), 
                    new NamedParameter("filePath", samplePath)
                });

            Program.MakeCloud(builder.Build()).Error.Should().Be("Not positive width or height");
        }
    }
}