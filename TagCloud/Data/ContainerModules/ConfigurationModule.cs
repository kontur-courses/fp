using System.Drawing;
using Autofac;
using TagCloud.ColorPicker;
using TagCloud.Counter;
using TagCloud.Drawer;
using TagCloud.RectanglesLayouter;
using TagCloud.RectanglesLayouter.PointsGenerator;
using TagCloud.Saver;
using TagCloud.WordsLayouter;

namespace TagCloud.Data.ContainerModules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TagCloudGenerator>().AsSelf();

            builder.RegisterModule<ReadersModule>();

            builder.RegisterType<CloudWordsLayouter>().As<IWordsLayouter>();
            builder.RegisterType<CloudDrawer>().As<IWordsDrawer>();
            builder.RegisterType<WordsCounter>().As<IWordsCounter>();
            builder.RegisterType<FileImageSaver>().As<IImageSaver>();
            builder.RegisterType<ClipboardImageSaver>().As<IImageSaver>();

            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.Register(c => new Point()).As<Point>();
            builder.Register(c => new SpiralPointsGenerator(1, 0.01)).As<IPointsGenerator>();

            builder.RegisterType<BrightnessColorPicker>().As<IColorPicker>();
        }
    }
}