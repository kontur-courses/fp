using Autofac;
using TagsCloudContainer;
using TagsCloudContainer.ClientsInterfaces;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;

namespace ContainerConfigurers
{
    public class AutofacConfigurer
    {
        private readonly ContainerBuilder builder;

        public AutofacConfigurer(IUserConfig userConfig)
        {
            builder = new ContainerBuilder();
            builder.Register(c => userConfig).As<IUserConfig>();
        }

        public IContainer GetContainer()
        {
            RegisterTextParser();
            RegisterPaintConfig();
            RegisterCloudLayouter();
            RegisterCloudPainter();
            return builder.Build();
        }

        private void RegisterCloudPainter()
        {
            builder.Register(c => new CloudPainter(
                    c.Resolve<ICloudLayouter>(),
                    c.Resolve<IPaintConfig>(),
                    c.Resolve<ITextParser>(),
                    ResolveFullSavePath(c)
                )).AsSelf()
                .SingleInstance();
        }

        private string ResolveFullSavePath(IComponentContext c)
        {
            return c.Resolve<IUserConfig>().OutputFilePath +
                   "\\" + c.Resolve<IUserConfig>().OutputFileName;
        }

        private void RegisterTextParser()
        {
            builder.Register(c => c.Resolve<IUserConfig>().TextParser)
                .As<ITextParser>().SingleInstance();
        }

        private void RegisterPaintConfig()
        {
            builder.Register(c => new PaintConfig(
                    c.Resolve<IUserConfig>().ColorScheme,
                    c.Resolve<IUserConfig>().ImageFormat,
                    c.Resolve<IUserConfig>().TagsFontName,
                    c.Resolve<IUserConfig>().TagsFontSize,
                    c.Resolve<IUserConfig>().ImageSize))
                .As<IPaintConfig>().SingleInstance();
        }

        private void RegisterCloudLayouter()
        {
            builder.Register(c => new CircularCloudLayouter(
                    c.Resolve<IUserConfig>().ImageCenter,
                    c.Resolve<IUserConfig>().Spiral))
                .As<ICloudLayouter>().SingleInstance();
        }
    }
}
