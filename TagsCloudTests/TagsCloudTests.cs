using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Core;
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
        [Test]
        public void CreateCloud_FromTxt()
        {
            var directoryInfo = Directory.GetCurrentDirectory();
            var samplePath = $"{directoryInfo}\\sample.png";
            new FileInfo(samplePath).Delete();
            
            var builder = new ContainerBuilder();

            builder.RegisterType<AllWordSelector>().As<IWordSelector>();
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));

            builder.RegisterType<ByCollectionBoringWordsDetector>().As<IBoringWordsDetector>();
            builder.RegisterType<StatisticProvider>().As<IStatisticProvider>();

            builder.RegisterInstance(new FontFamily("Arial"));
            builder.RegisterType<SpiralPoints>().As<IPointsLayout>();
            builder.RegisterType<WordLayouter>().SingleInstance().As<IWordLayouter>();

            builder.RegisterInstance(new[]
                {Color.Black, Color.Red, Color.Blue, Color.Green, Color.Yellow});
            builder.RegisterType<RandomColorSelector>().SingleInstance().As<IColorSelector>();
            
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
    }
}