using FluentAssertions;
using LightInject;
using NUnit.Framework;
using System.IO;
using TagsCloudApp.Providers;
using TagsCloudContainer.Appliers;
using TagsCloudContainer.BitmapSavers;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Parsers;
using TagsCloudContainer.TagCloudPainters;
using TagsCloudContainer.TagCreators;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainerTests
{
    internal class CloudImageGeneratorTests
    {
        [Test]
        public void Should_SaveToFile()
        {
            var container = ContainerProvider.GetContainer();
            var textPath = Path.Combine(Path.GetFullPath(@"..\..\..\texts"), "test.txt");
            var parser = container.GetInstance<IParser>("1");
            var preprocessorsApplier = container.GetInstance<IPreprocessorsApplier>();
            var filtersApplier = container.GetInstance<IFiltersApplier>();
            var tagCreator = container.GetInstance<ITagCreator>();
            var tagPainter = container.GetInstance<ITagPainter>("1");
            var cloudTagCreator = container.GetInstance<ICloudTagCreator>();
            var cloudPainter = container.GetInstance<ITagCloudPainter>();
            var bitmapSaver = container.GetInstance<IBitmapSaver>();
            var path = parser.Parse(textPath)
                .Then(preprocessorsApplier.ApplyPreprocessors)
                .Then(filtersApplier.ApplyFilters)
                .Then(tagCreator.CreateTags)
                .Then(tagPainter.PaintTags)
                .Then(cloudTagCreator.CreateCloudTags)
                .Then(cloudPainter.Paint)
                .Then(bitmapSaver.SaveBitmap).Value;

            File.Exists(path).Should().BeTrue();
            File.Delete(path);
        }
    }
}
