using System;
using Autofac;
using TagsCloud.Visualization.ColorGenerators;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.FontFactories;
using TagsCloud.Visualization.ImagesSavers;
using TagsCloud.Visualization.LayoutContainer.ContainerBuilder;
using TagsCloud.Visualization.PointGenerator;
using TagsCloud.Visualization.TagsCloudVisualizers;
using TagsCloud.Visualization.WordsFilters;
using TagsCloud.Visualization.WordsParsers;
using TagsCloud.Visualization.WordsSizeServices;

namespace TagsCloud.Visualization
{
    public class TagsCloudModule : Module
    {
        private readonly TagsCloudModuleSettings settings;

        public TagsCloudModule(TagsCloudModuleSettings settings)
            => this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WordsService>().As<IWordsService>();

            builder.RegisterType<WordLengthFilter>().As<IWordsFilter>();
            builder.RegisterType<WordsParser>().As<IWordsParser>();

            builder.Register(_ => new FontFactory(settings.FontSettings ?? new FontSettings())).As<IFontFactory>();
            builder.RegisterType<WordsSizeService>().As<IWordsSizeService>();
            builder.RegisterType<TagsContainerBuilder>().As<AbstractTagsContainerBuilder>();

            builder.Register(_ => new ArchimedesSpiralPointGenerator(settings.Center)).As<IPointGenerator>();
            builder.RegisterType(settings.LayouterType).As<ICloudLayouter>();

            builder.Register(_ => settings.ColorGenerator).As<IColorGenerator>();
            builder.RegisterType<Drawer>().As<IDrawer>();
            builder.Register(_ => new ImageSaver(settings.SaveSettings)).As<IImageSaver>();

            builder.RegisterType<TagsCloudVisualizer>().As<ITagsCloudVisualizer>();
        }
    }
}