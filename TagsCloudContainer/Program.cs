using Autofac;
using System.Reflection;
using TagsCloudContainer.Actions;
using TagsCloudContainer.Algorithm;
using TagsCloudContainer.Client;
using TagsCloudContainer.Infrastucture.Settings;
using TagsCloudContainer.Infrastucture.UiActions;
using TagsCloudContainer.Infrastucture.Visualization;

namespace TagsCloudContainer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                var builder = CreateBuilder();
                var container = builder.Build();

                ApplicationConfiguration.Initialize();
                Application.Run(container.Resolve<Form>());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static ContainerBuilder CreateBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainForm>().As<Form>().SingleInstance();
            builder.RegisterType<FileParser>().As<IFileParser>().SingleInstance();
            builder.RegisterType<WordProcessor>().As<IWordProcessor>().SingleInstance();
            builder.RegisterType<GUITagCloudClient>().As<ITagCloudClient>().SingleInstance();
            builder.RegisterType<TagCloudDrawer>().As<IDrawer>().SingleInstance();
            builder.RegisterType<ImageSettings>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<FileSettings>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AlgorithmSettings>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PictureBox>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().InstancePerLifetimeScope();
            builder.RegisterType<RectanglePlacer>().As<IRectanglePlacer>().InstancePerLifetimeScope();
            builder.RegisterType<CloudSizer>().As<ICloudSizer>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IUiAction).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            return builder;
        }
    }
}